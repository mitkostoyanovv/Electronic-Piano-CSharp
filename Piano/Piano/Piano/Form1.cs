using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;
using NAudio.Wave;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PianoProject
{
    public partial class MainForm : Form
    {
        // ---------- Клавиатура ----------
        private List<Button> whiteKeys = new List<Button>();
        private List<Button> blackKeys = new List<Button>();

        // ---------- MIDI ----------
        private OutputDevice midiOut;
        private const int midiChannel = 0;       // MIDI канал

        // ---------- Октави ----------
        private int currentOctave = 5;
        private readonly int[] allowedOctaves = { 4, 5, 6 };

        // ---------- Карти за клавишите от компютърната клавиатура ----------
        private readonly Dictionary<Keys, int> whiteKeyMap = new Dictionary<Keys, int>()
        {
            { Keys.A, 0 }, { Keys.S, 2 }, { Keys.D, 4 }, { Keys.F, 5 },
            { Keys.G, 7 }, { Keys.H, 9 }, { Keys.J, 11 }
        };

        private readonly Dictionary<Keys, int> blackKeyMap = new Dictionary<Keys, int>()
        {
            { Keys.W, 1 }, { Keys.E, 3 }, { Keys.T, 6 }, { Keys.Y, 8 }, { Keys.U, 10 }
        };

        // Активно натиснати ноти (за да не се дублират)
        private HashSet<int> activeNotes = new HashSet<int>();

        // ---------- Запис и възпроизвеждане ----------
        private List<RecordedMidiEvent> recordedEvents = new List<RecordedMidiEvent>();
        private Stopwatch stopwatch = new Stopwatch();
        private bool isRecording = false;
        private bool isExporting = false;
        private bool isPlayingBack = false;
        private CancellationTokenSource playbackCancellation;

        // Структура за записано MIDI събитие
        private struct RecordedMidiEvent
        {
            public long TimeMs;
            public ChannelMessage Message;
        }

        // ---------- Конструктор ----------
        public MainForm()
        {
            InitializeComponent();
            KeyPreview = true;                     // За да прихващаме клавишите преди другите контроли

            // Инициализация на MIDI изход (първо MIDI устройство)
            midiOut = new OutputDevice(0);

            // Настройки по подразбиране
            comboInstrument.SelectedIndex = 0;
            lblOctave.Text = "C" + currentOctave;

            // При промяна на размера на панела – генерирай клавиатурата наново
            panelKeyboard.Resize += (s, e) => GenerateKeyboard();
            GenerateKeyboard();

            // Свързване на събитията с методи (вече са зададени в дизайнера, но за по-ясно ги добавяме и тук)
            btnOctaveUp.Click += btnOctaveUp_Click;
            btnOctaveDown.Click += btnOctaveDown_Click;
            comboInstrument.SelectedIndexChanged += comboInstrument_SelectedIndexChanged;
            chkShowNotes.CheckedChanged += chkShowNotes_CheckedChanged;
            trackVolume.ValueChanged += TrackVolume_ValueChanged;

            btnRecord.Click += btnRecord_Click;
            btnStop.Click += btnStop_Click;
            btnPlay.Click += btnPlay_Click;
            btnExport.Click += btnExport_Click;

            this.FormClosing += MainForm_FormClosing;
        }

        // ---------- Управление на звука ----------
        private void TrackVolume_ValueChanged(object sender, EventArgs e)
        {
            // Закръгляне до десетици (за по-лесно визуално)
            int v = trackVolume.Value;
            int rounded = (int)Math.Round(v / 10.0) * 10;
            if (rounded != v) trackVolume.Value = rounded;
        }

        // ---------- Генериране на визуалната клавиатура ----------
        private void GenerateKeyboard()
        {
            panelKeyboard.Controls.Clear();
            whiteKeys.Clear();
            blackKeys.Clear();

            string[] whiteNotes = { "C", "D", "E", "F", "G", "A", "B" };
            string[] blackNotes = { "C#", "D#", "F#", "G#", "A#" };
            int[] blackPosition = { 0, 1, 3, 4, 5 }; // Индекс на левия бял клавиш, между който стои черният

            int keyWidth = panelKeyboard.Width / whiteNotes.Length;
            int whiteHeight = panelKeyboard.Height;
            int blackHeight = (int)(whiteHeight * 0.6);
            Font keyFont = new Font("Segoe UI", 10F, FontStyle.Regular);

            // Бели клавиши
            for (int i = 0; i < whiteNotes.Length; i++)
            {
                Button key = new Button
                {
                    Width = keyWidth,
                    Height = whiteHeight,
                    BackColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Margin = new Padding(0),
                    Tag = whiteNotes[i] + currentOctave,
                    TextAlign = ContentAlignment.BottomCenter,
                    Font = keyFont
                };
                if (chkShowNotes.Checked) key.Text = whiteNotes[i];

                key.MouseDown += PianoKeyDown;
                key.MouseUp += PianoKeyUp;
                key.Location = new Point(i * keyWidth, 0);
                panelKeyboard.Controls.Add(key);
                whiteKeys.Add(key);
            }

            // Черни клавиши – позиционират се точно между два бели
            for (int i = 0; i < blackNotes.Length; i++)
            {
                Button blackKey = new Button
                {
                    Width = (int)(keyWidth * 0.6),
                    Height = blackHeight,
                    BackColor = Color.Black,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Tag = blackNotes[i] + currentOctave,
                    TextAlign = ContentAlignment.BottomCenter,
                    Font = keyFont
                };
                if (chkShowNotes.Checked) blackKey.Text = blackNotes[i];

                int leftIndex = blackPosition[i];
                int rightIndex = leftIndex + 1;

                int leftCenter = whiteKeys[leftIndex].Left + whiteKeys[leftIndex].Width / 2;
                int rightCenter = whiteKeys[rightIndex].Left + whiteKeys[rightIndex].Width / 2;
                int x = (leftCenter + rightCenter) / 2 - blackKey.Width / 2;

                blackKey.Location = new Point(x, 0);
                blackKey.MouseDown += PianoKeyDown;
                blackKey.MouseUp += PianoKeyUp;

                panelKeyboard.Controls.Add(blackKey);
                blackKey.BringToFront();
                blackKeys.Add(blackKey);
            }
        }

        // ---------- Помощни функции за MIDI ноти ----------
        private int GetMidiNote(string noteWithOctave)
        {
            // noteWithOctave e във формат "C5", "C#5" и т.н.
            string noteName = noteWithOctave.Substring(0, noteWithOctave.Length - 1);
            int octave = int.Parse(noteWithOctave[noteWithOctave.Length - 1].ToString());

            // Карта от имена на ноти към MIDI номера (0 = C, 1 = C# и т.н.)
            var noteMap = new Dictionary<string, int>()
            {
                { "C", 0 }, { "C#", 1 }, { "D", 2 }, { "D#", 3 }, { "E", 4 },
                { "F", 5 }, { "F#", 6 }, { "G", 7 }, { "G#", 8 }, { "A", 9 },
                { "A#", 10 }, { "B", 11 }
            };
            return 12 * (octave + 1) + noteMap[noteName];
        }

        private Button FindKeyByMidiNote(int midiNote)
        {
            foreach (Button b in whiteKeys)
                if (GetMidiNote(b.Tag.ToString()) == midiNote) return b;
            foreach (Button b in blackKeys)
                if (GetMidiNote(b.Tag.ToString()) == midiNote) return b;
            return null;
        }

        private void HighlightKey(Button key, bool pressed)
        {
            if (key == null) return;
            if (pressed)
                key.BackColor = key.BackColor == Color.White ? Color.LightBlue : Color.DarkBlue;
            else
                key.BackColor = blackKeys.Contains(key) ? Color.Black : Color.White;
        }

        // ---------- Възпроизвеждане на нота (MIDI) ----------
        private void PlayMidiNote(int note, int velocity, bool noteOn)
        {
            var command = noteOn ? ChannelCommand.NoteOn : ChannelCommand.NoteOff;
            midiOut.Send(new ChannelMessage(command, midiChannel, note, velocity));
        }

        // ---------- Събития от мишката върху клавишите ----------
        private void PianoKeyDown(object sender, MouseEventArgs e)
        {
            if (isExporting || isPlayingBack) return;

            Button key = sender as Button;
            if (key?.Tag == null) return;

            int note = GetMidiNote(key.Tag.ToString());
            HighlightKey(key, true);

            if (!activeNotes.Contains(note))
            {
                PlayMidiNote(note, trackVolume.Value, true);
                activeNotes.Add(note);

                if (isRecording)
                {
                    recordedEvents.Add(new RecordedMidiEvent
                    {
                        TimeMs = stopwatch.ElapsedMilliseconds,
                        Message = new ChannelMessage(ChannelCommand.NoteOn, midiChannel, note, trackVolume.Value)
                    });
                }
            }
        }

        private void PianoKeyUp(object sender, MouseEventArgs e)
        {
            if (isExporting || isPlayingBack) return;

            Button key = sender as Button;
            if (key?.Tag == null) return;

            int note = GetMidiNote(key.Tag.ToString());
            HighlightKey(key, false);

            if (activeNotes.Contains(note))
            {
                PlayMidiNote(note, 0, false);
                activeNotes.Remove(note);

                if (isRecording)
                {
                    recordedEvents.Add(new RecordedMidiEvent
                    {
                        TimeMs = stopwatch.ElapsedMilliseconds,
                        Message = new ChannelMessage(ChannelCommand.NoteOff, midiChannel, note, 0)
                    });
                }
            }
        }

        // ---------- Събития от компютърната клавиатура ----------
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (isExporting || isPlayingBack) return;

            int midiNote = -1;
            if (whiteKeyMap.ContainsKey(e.KeyCode))
                midiNote = 12 * (currentOctave + 1) + whiteKeyMap[e.KeyCode];
            else if (blackKeyMap.ContainsKey(e.KeyCode))
                midiNote = 12 * (currentOctave + 1) + blackKeyMap[e.KeyCode];

            if (midiNote != -1 && !activeNotes.Contains(midiNote))
            {
                PlayMidiNote(midiNote, trackVolume.Value, true);
                activeNotes.Add(midiNote);
                HighlightKey(FindKeyByMidiNote(midiNote), true);

                if (isRecording)
                {
                    recordedEvents.Add(new RecordedMidiEvent
                    {
                        TimeMs = stopwatch.ElapsedMilliseconds,
                        Message = new ChannelMessage(ChannelCommand.NoteOn, midiChannel, midiNote, trackVolume.Value)
                    });
                }
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (isExporting || isPlayingBack) return;

            int midiNote = -1;
            if (whiteKeyMap.ContainsKey(e.KeyCode))
                midiNote = 12 * (currentOctave + 1) + whiteKeyMap[e.KeyCode];
            else if (blackKeyMap.ContainsKey(e.KeyCode))
                midiNote = 12 * (currentOctave + 1) + blackKeyMap[e.KeyCode];

            if (midiNote != -1 && activeNotes.Contains(midiNote))
            {
                PlayMidiNote(midiNote, 0, false);
                activeNotes.Remove(midiNote);
                HighlightKey(FindKeyByMidiNote(midiNote), false);

                if (isRecording)
                {
                    recordedEvents.Add(new RecordedMidiEvent
                    {
                        TimeMs = stopwatch.ElapsedMilliseconds,
                        Message = new ChannelMessage(ChannelCommand.NoteOff, midiChannel, midiNote, 0)
                    });
                }
            }
        }

        // ---------- Промяна на октава ----------
        private void btnOctaveUp_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(allowedOctaves, currentOctave);
            if (index < allowedOctaves.Length - 1)
                currentOctave = allowedOctaves[index + 1];
            lblOctave.Text = "C" + currentOctave;
            GenerateKeyboard();
            activeNotes.Clear();  // Всички активни ноти се нулират
        }

        private void btnOctaveDown_Click(object sender, EventArgs e)
        {
            int index = Array.IndexOf(allowedOctaves, currentOctave);
            if (index > 0)
                currentOctave = allowedOctaves[index - 1];
            lblOctave.Text = "C" + currentOctave;
            GenerateKeyboard();
            activeNotes.Clear();
        }

        // ---------- Показване на имена на нотите ----------
        private void chkShowNotes_CheckedChanged(object sender, EventArgs e) => GenerateKeyboard();

        // ---------- Смяна на инструмент (MIDI program change) ----------
        private void SetInstrument(int program)
        {
            midiOut.Send(new ChannelMessage(ChannelCommand.ProgramChange, midiChannel, program, 0));
        }

        private void comboInstrument_SelectedIndexChanged(object sender, EventArgs e)
        {
            int[] instruments = { 0, 19, 56, 73, 40 };
            SetInstrument(instruments[comboInstrument.SelectedIndex]);
        }

        // ---------- Запис ----------
        private void btnRecord_Click(object sender, EventArgs e)
        {
            recordedEvents.Clear();
            stopwatch.Reset();
            stopwatch.Start();
            isRecording = true;

            btnRecord.Enabled = false;
            btnStop.Enabled = true;
            btnPlay.Enabled = false;
            btnExport.Enabled = false;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (isRecording)
            {
                stopwatch.Stop();
                isRecording = false;

                btnRecord.Enabled = true;
                btnStop.Enabled = false;
                btnPlay.Enabled = recordedEvents.Count > 0;
                btnExport.Enabled = recordedEvents.Count > 0;
            }
        }

        // ---------- Възпроизвеждане на записа ----------
        private async void btnPlay_Click(object sender, EventArgs e)
        {
            if (isPlayingBack)
            {
                playbackCancellation?.Cancel();
                return;
            }

            if (recordedEvents.Count == 0) return;

            int program = GetSelectedInstrumentProgram();
            playbackCancellation = new CancellationTokenSource();
            var token = playbackCancellation.Token;

            isPlayingBack = true;
            btnPlay.Text = "Stop";
            btnRecord.Enabled = false;
            btnStop.Enabled = false;
            btnExport.Enabled = false;

            try
            {
                await Task.Run(() => PlaybackEvents(program, token), token);
            }
            catch (OperationCanceledException)
            {
                // Потребителят е натиснал Stop
            }
            finally
            {
                isPlayingBack = false;
                btnPlay.Text = "Play";
                btnRecord.Enabled = !isRecording;
                btnStop.Enabled = isRecording;
                btnPlay.Enabled = recordedEvents.Count > 0;
                btnExport.Enabled = recordedEvents.Count > 0;

                // Изпращаме All Notes Off, за да спрем евентуално залепнали звуци
                midiOut.Send(new ChannelMessage(ChannelCommand.Controller, midiChannel, 123, 0));
                playbackCancellation?.Dispose();
                playbackCancellation = null;
            }
        }

        private void PlaybackEvents(int program, CancellationToken token)
        {
            // Задаваме инструмента
            midiOut.Send(new ChannelMessage(ChannelCommand.ProgramChange, midiChannel, program, 0));

            var sortedEvents = recordedEvents.OrderBy(e => e.TimeMs).ToList();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            foreach (var ev in sortedEvents)
            {
                token.ThrowIfCancellationRequested();

                long targetMs = ev.TimeMs;
                while (sw.ElapsedMilliseconds < targetMs)
                {
                    long remaining = targetMs - sw.ElapsedMilliseconds;
                    if (remaining > 15)
                        Thread.Sleep((int)(remaining - 5));
                    else
                        Thread.SpinWait(100);
                    token.ThrowIfCancellationRequested();
                }
                midiOut.Send(ev.Message);
            }

            sw.Stop();
            Thread.Sleep(300); // Малко изчакване за последните ноти
        }

        // ---------- Експорт към WAV ----------
        private async void btnExport_Click(object sender, EventArgs e)
        {
            if (recordedEvents.Count == 0)
            {
                MessageBox.Show("Няма записани ноти.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "WAV files (*.wav)|*.wav";
            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            string outputFile = sfd.FileName;
            int program = GetSelectedInstrumentProgram();

            isExporting = true;
            btnExport.Enabled = false;
            btnRecord.Enabled = false;
            btnStop.Enabled = false;
            btnPlay.Enabled = false;

            WasapiLoopbackCapture capture = null;
            WaveFileWriter writer = null;

            try
            {
                capture = new WasapiLoopbackCapture();
                writer = new WaveFileWriter(outputFile, capture.WaveFormat);

                capture.DataAvailable += (s, args) =>
                {
                    writer.Write(args.Buffer, 0, args.BytesRecorded);
                };

                capture.RecordingStopped += (s, args) =>
                {
                    writer?.Dispose();
                    writer = null;
                    capture?.Dispose();
                };

                capture.StartRecording();

                // Възпроизвеждаме записа, докато записваме от звуковата карта
                await Task.Run(() => PlaybackEvents(program, CancellationToken.None));

                // Изчакваме малко повече, за да уловим остатъчния звук
                await Task.Delay(2000);

                capture.StopRecording();

                MessageBox.Show("WAV файлът е създаден успешно!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Грешка при експорт: " + ex.Message, "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                capture?.StopRecording();
            }
            finally
            {
                isExporting = false;
                btnRecord.Enabled = !isRecording;
                btnStop.Enabled = isRecording;
                btnPlay.Enabled = recordedEvents.Count > 0;
                btnExport.Enabled = recordedEvents.Count > 0;
            }
        }

        // Помощна функция за взимане на номера на избрания инструмент
        private int GetSelectedInstrumentProgram()
        {
            int[] instruments = { 0, 19, 56, 73, 40 };
            return instruments[comboInstrument.SelectedIndex];
        }

        // ---------- Почистване при затваряне ----------
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isRecording)
            {
                stopwatch.Stop();
                isRecording = false;
            }
            if (isPlayingBack)
            {
                playbackCancellation?.Cancel();
            }
        }
    }
}