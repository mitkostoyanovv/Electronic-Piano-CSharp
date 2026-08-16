namespace PianoProject
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing != null && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelTop = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnOctaveDown = new System.Windows.Forms.Button();
            this.btnOctaveUp = new System.Windows.Forms.Button();
            this.lblOctave = new System.Windows.Forms.Label();
            this.comboInstrument = new System.Windows.Forms.ComboBox();
            this.chkShowNotes = new System.Windows.Forms.CheckBox();
            this.btnRecord = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.panelSettings = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.trackVolume = new System.Windows.Forms.TrackBar();
            this.panelKeyboard = new System.Windows.Forms.Panel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panelTop.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panelSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackVolume)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.LightGray;
            this.panelTop.Controls.Add(this.flowLayoutPanel1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(10);
            this.panelTop.Size = new System.Drawing.Size(933, 80);
            this.panelTop.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnOctaveDown);
            this.flowLayoutPanel1.Controls.Add(this.btnOctaveUp);
            this.flowLayoutPanel1.Controls.Add(this.lblOctave);
            this.flowLayoutPanel1.Controls.Add(this.comboInstrument);
            this.flowLayoutPanel1.Controls.Add(this.chkShowNotes);
            this.flowLayoutPanel1.Controls.Add(this.btnRecord);
            this.flowLayoutPanel1.Controls.Add(this.btnStop);
            this.flowLayoutPanel1.Controls.Add(this.btnPlay);
            this.flowLayoutPanel1.Controls.Add(this.btnExport);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(10, 10);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(913, 60);
            this.flowLayoutPanel1.TabIndex = 0;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // btnOctaveDown
            // 
            this.btnOctaveDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOctaveDown.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnOctaveDown.Location = new System.Drawing.Point(3, 12);
            this.btnOctaveDown.Margin = new System.Windows.Forms.Padding(3, 12, 0, 3);
            this.btnOctaveDown.Name = "btnOctaveDown";
            this.btnOctaveDown.Size = new System.Drawing.Size(50, 35);
            this.btnOctaveDown.TabIndex = 0;
            this.btnOctaveDown.Text = "◀";
            this.toolTip.SetToolTip(this.btnOctaveDown, "Lower octave");
            this.btnOctaveDown.UseVisualStyleBackColor = true;
            // 
            // btnOctaveUp
            // 
            this.btnOctaveUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOctaveUp.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnOctaveUp.Location = new System.Drawing.Point(53, 12);
            this.btnOctaveUp.Margin = new System.Windows.Forms.Padding(0, 12, 10, 3);
            this.btnOctaveUp.Name = "btnOctaveUp";
            this.btnOctaveUp.Size = new System.Drawing.Size(50, 35);
            this.btnOctaveUp.TabIndex = 1;
            this.btnOctaveUp.Text = "▶";
            this.toolTip.SetToolTip(this.btnOctaveUp, "Higher octave");
            this.btnOctaveUp.UseVisualStyleBackColor = true;
            // 
            // lblOctave
            // 
            this.lblOctave.AutoSize = true;
            this.lblOctave.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblOctave.Location = new System.Drawing.Point(116, 17);
            this.lblOctave.Margin = new System.Windows.Forms.Padding(3, 17, 10, 3);
            this.lblOctave.Name = "lblOctave";
            this.lblOctave.Size = new System.Drawing.Size(35, 25);
            this.lblOctave.TabIndex = 2;
            this.lblOctave.Text = "C5";
            this.toolTip.SetToolTip(this.lblOctave, "Current octave");
            // 
            // comboInstrument
            // 
            this.comboInstrument.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboInstrument.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.comboInstrument.FormattingEnabled = true;
            this.comboInstrument.Items.AddRange(new object[] {
            "Piano",
            "Organ",
            "Trumpet",
            "Flute",
            "Violin"});
            this.comboInstrument.Location = new System.Drawing.Point(164, 17);
            this.comboInstrument.Margin = new System.Windows.Forms.Padding(3, 17, 10, 3);
            this.comboInstrument.Name = "comboInstrument";
            this.comboInstrument.Size = new System.Drawing.Size(140, 25);
            this.comboInstrument.TabIndex = 3;
            this.toolTip.SetToolTip(this.comboInstrument, "Select instrument");
            // 
            // chkShowNotes
            // 
            this.chkShowNotes.AutoSize = true;
            this.chkShowNotes.Checked = true;
            this.chkShowNotes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowNotes.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkShowNotes.Location = new System.Drawing.Point(317, 17);
            this.chkShowNotes.Margin = new System.Windows.Forms.Padding(3, 17, 10, 3);
            this.chkShowNotes.Name = "chkShowNotes";
            this.chkShowNotes.Size = new System.Drawing.Size(99, 23);
            this.chkShowNotes.TabIndex = 4;
            this.chkShowNotes.Text = "Show notes";
            this.toolTip.SetToolTip(this.chkShowNotes, "Show note names on keys");
            this.chkShowNotes.UseVisualStyleBackColor = true;
            // 
            // btnRecord
            // 
            this.btnRecord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecord.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRecord.Location = new System.Drawing.Point(429, 12);
            this.btnRecord.Margin = new System.Windows.Forms.Padding(3, 12, 5, 3);
            this.btnRecord.Name = "btnRecord";
            this.btnRecord.Size = new System.Drawing.Size(70, 35);
            this.btnRecord.TabIndex = 5;
            this.btnRecord.Text = "Record";
            this.toolTip.SetToolTip(this.btnRecord, "Start recording");
            this.btnRecord.UseVisualStyleBackColor = true;
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnStop.Location = new System.Drawing.Point(504, 12);
            this.btnStop.Margin = new System.Windows.Forms.Padding(0, 12, 5, 3);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(70, 35);
            this.btnStop.TabIndex = 6;
            this.btnStop.Text = "Stop";
            this.toolTip.SetToolTip(this.btnStop, "Stop recording");
            this.btnStop.UseVisualStyleBackColor = true;
            // 
            // btnPlay
            // 
            this.btnPlay.Enabled = false;
            this.btnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlay.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnPlay.Location = new System.Drawing.Point(579, 12);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(0, 12, 5, 3);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(70, 35);
            this.btnPlay.TabIndex = 8;
            this.btnPlay.Text = "Play";
            this.toolTip.SetToolTip(this.btnPlay, "Play recorded");
            this.btnPlay.UseVisualStyleBackColor = true;
            // 
            // btnExport
            // 
            this.btnExport.Enabled = false;
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnExport.Location = new System.Drawing.Point(654, 12);
            this.btnExport.Margin = new System.Windows.Forms.Padding(0, 12, 3, 3);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(90, 35);
            this.btnExport.TabIndex = 7;
            this.btnExport.Text = "Export WAV";
            this.toolTip.SetToolTip(this.btnExport, "Save as WAV");
            this.btnExport.UseVisualStyleBackColor = true;
            // 
            // panelSettings
            // 
            this.panelSettings.BackColor = System.Drawing.Color.White;
            this.panelSettings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSettings.Controls.Add(this.label1);
            this.panelSettings.Controls.Add(this.trackVolume);
            this.panelSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSettings.Location = new System.Drawing.Point(0, 80);
            this.panelSettings.Name = "panelSettings";
            this.panelSettings.Padding = new System.Windows.Forms.Padding(15);
            this.panelSettings.Size = new System.Drawing.Size(933, 80);
            this.panelSettings.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Volume";
            this.toolTip.SetToolTip(this.label1, "Volume level");
            // 
            // trackVolume
            // 
            this.trackVolume.LargeChange = 10;
            this.trackVolume.Location = new System.Drawing.Point(15, 37);
            this.trackVolume.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.trackVolume.Maximum = 100;
            this.trackVolume.Name = "trackVolume";
            this.trackVolume.Size = new System.Drawing.Size(350, 45);
            this.trackVolume.SmallChange = 10;
            this.trackVolume.TabIndex = 1;
            this.trackVolume.TickFrequency = 10;
            this.toolTip.SetToolTip(this.trackVolume, "Adjust volume");
            this.trackVolume.Value = 100;
            // 
            // panelKeyboard
            // 
            this.panelKeyboard.BackColor = System.Drawing.Color.DarkGray;
            this.panelKeyboard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelKeyboard.Location = new System.Drawing.Point(0, 160);
            this.panelKeyboard.Name = "panelKeyboard";
            this.panelKeyboard.Size = new System.Drawing.Size(933, 411);
            this.panelKeyboard.TabIndex = 2;
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 300;
            this.toolTip.ShowAlways = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(933, 571);
            this.Controls.Add(this.panelKeyboard);
            this.Controls.Add(this.panelSettings);
            this.Controls.Add(this.panelTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Piano";
            this.panelTop.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.panelSettings.ResumeLayout(false);
            this.panelSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackVolume)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnOctaveDown;
        private System.Windows.Forms.Button btnOctaveUp;
        private System.Windows.Forms.Label lblOctave;
        private System.Windows.Forms.ComboBox comboInstrument;
        private System.Windows.Forms.CheckBox chkShowNotes;
        private System.Windows.Forms.Button btnRecord;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Panel panelSettings;
        private System.Windows.Forms.TrackBar trackVolume;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelKeyboard;
        private System.Windows.Forms.ToolTip toolTip;
    }
}