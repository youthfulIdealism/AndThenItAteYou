namespace Survive
{
    partial class Launcher
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Launcher));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.start_game_button = new System.Windows.Forms.Button();
            this.saveFileBox = new System.Windows.Forms.GroupBox();
            this.saveSlot = new System.Windows.Forms.ComboBox();
            this.difficultyBox = new System.Windows.Forms.GroupBox();
            this.difficulty_very_hard = new System.Windows.Forms.RadioButton();
            this.difficulty_hard = new System.Windows.Forms.RadioButton();
            this.audioBox = new System.Windows.Forms.GroupBox();
            this.button_audio_test = new System.Windows.Forms.Button();
            this.lable_sound_monster = new System.Windows.Forms.Label();
            this.trackbar_sound_monster = new System.Windows.Forms.TrackBar();
            this.label_sound_music = new System.Windows.Forms.Label();
            this.trackbar_sound_music = new System.Windows.Forms.TrackBar();
            this.label_sound_ambience = new System.Windows.Forms.Label();
            this.trackbar_sound_ambient = new System.Windows.Forms.TrackBar();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.ScreenSettingsGroup = new System.Windows.Forms.GroupBox();
            this.screenSettingsLabel = new System.Windows.Forms.Label();
            this.heightLabel = new System.Windows.Forms.Label();
            this.widthLabel = new System.Windows.Forms.Label();
            this.fullscreenOptionsBox = new System.Windows.Forms.ComboBox();
            this.screenHBox = new System.Windows.Forms.TextBox();
            this.screenWBox = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.saveFileBox.SuspendLayout();
            this.difficultyBox.SuspendLayout();
            this.audioBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackbar_sound_monster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackbar_sound_music)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackbar_sound_ambient)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.ScreenSettingsGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.start_game_button);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.saveFileBox);
            this.splitContainer1.Panel2.Controls.Add(this.difficultyBox);
            this.splitContainer1.Panel2.Controls.Add(this.audioBox);
            this.splitContainer1.Panel2.Controls.Add(this.flowLayoutPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(459, 494);
            this.splitContainer1.SplitterDistance = 175;
            this.splitContainer1.TabIndex = 0;
            // 
            // start_game_button
            // 
            this.start_game_button.Location = new System.Drawing.Point(94, 32);
            this.start_game_button.Name = "start_game_button";
            this.start_game_button.Size = new System.Drawing.Size(260, 70);
            this.start_game_button.TabIndex = 0;
            this.start_game_button.Text = "Start Game";
            this.start_game_button.UseVisualStyleBackColor = true;
            this.start_game_button.Click += new System.EventHandler(this.start_game_button_Click);
            // 
            // saveFileBox
            // 
            this.saveFileBox.Controls.Add(this.saveSlot);
            this.saveFileBox.Location = new System.Drawing.Point(125, 215);
            this.saveFileBox.Name = "saveFileBox";
            this.saveFileBox.Size = new System.Drawing.Size(322, 90);
            this.saveFileBox.TabIndex = 3;
            this.saveFileBox.TabStop = false;
            this.saveFileBox.Text = "Save Slot";
            // 
            // saveSlot
            // 
            this.saveSlot.FormattingEnabled = true;
            this.saveSlot.Items.AddRange(new object[] {
            "Slot 0",
            "Slot 1",
            "Slot 2",
            "Slot 3",
            "Slot 4",
            "Slot 5"});
            this.saveSlot.Location = new System.Drawing.Point(17, 30);
            this.saveSlot.Name = "saveSlot";
            this.saveSlot.Size = new System.Drawing.Size(121, 21);
            this.saveSlot.TabIndex = 0;
            this.saveSlot.SelectedIndexChanged += new System.EventHandler(this.saveSlot_SelectedIndexChanged);
            // 
            // difficultyBox
            // 
            this.difficultyBox.Controls.Add(this.difficulty_very_hard);
            this.difficultyBox.Controls.Add(this.difficulty_hard);
            this.difficultyBox.Location = new System.Drawing.Point(3, 209);
            this.difficultyBox.Margin = new System.Windows.Forms.Padding(2);
            this.difficultyBox.Name = "difficultyBox";
            this.difficultyBox.Padding = new System.Windows.Forms.Padding(2);
            this.difficultyBox.Size = new System.Drawing.Size(116, 96);
            this.difficultyBox.TabIndex = 2;
            this.difficultyBox.TabStop = false;
            this.difficultyBox.Text = "Difficulty";
            // 
            // difficulty_very_hard
            // 
            this.difficulty_very_hard.AutoSize = true;
            this.difficulty_very_hard.Location = new System.Drawing.Point(19, 58);
            this.difficulty_very_hard.Margin = new System.Windows.Forms.Padding(2);
            this.difficulty_very_hard.Name = "difficulty_very_hard";
            this.difficulty_very_hard.Size = new System.Drawing.Size(72, 17);
            this.difficulty_very_hard.TabIndex = 1;
            this.difficulty_very_hard.TabStop = true;
            this.difficulty_very_hard.Text = "Very Hard";
            this.difficulty_very_hard.UseVisualStyleBackColor = true;
            // 
            // difficulty_hard
            // 
            this.difficulty_hard.AutoSize = true;
            this.difficulty_hard.Location = new System.Drawing.Point(19, 36);
            this.difficulty_hard.Margin = new System.Windows.Forms.Padding(2);
            this.difficulty_hard.Name = "difficulty_hard";
            this.difficulty_hard.Size = new System.Drawing.Size(48, 17);
            this.difficulty_hard.TabIndex = 0;
            this.difficulty_hard.TabStop = true;
            this.difficulty_hard.Text = "Hard";
            this.difficulty_hard.UseVisualStyleBackColor = true;
            // 
            // audioBox
            // 
            this.audioBox.Controls.Add(this.button_audio_test);
            this.audioBox.Controls.Add(this.lable_sound_monster);
            this.audioBox.Controls.Add(this.trackbar_sound_monster);
            this.audioBox.Controls.Add(this.label_sound_music);
            this.audioBox.Controls.Add(this.trackbar_sound_music);
            this.audioBox.Controls.Add(this.label_sound_ambience);
            this.audioBox.Controls.Add(this.trackbar_sound_ambient);
            this.audioBox.Location = new System.Drawing.Point(3, 109);
            this.audioBox.Name = "audioBox";
            this.audioBox.Size = new System.Drawing.Size(453, 90);
            this.audioBox.TabIndex = 1;
            this.audioBox.TabStop = false;
            this.audioBox.Text = "Audio";
            // 
            // button_audio_test
            // 
            this.button_audio_test.Location = new System.Drawing.Point(363, 38);
            this.button_audio_test.Name = "button_audio_test";
            this.button_audio_test.Size = new System.Drawing.Size(75, 23);
            this.button_audio_test.TabIndex = 6;
            this.button_audio_test.Text = "Play";
            this.button_audio_test.UseVisualStyleBackColor = true;
            this.button_audio_test.Click += new System.EventHandler(this.button_test_audio_click);
            // 
            // lable_sound_monster
            // 
            this.lable_sound_monster.AutoSize = true;
            this.lable_sound_monster.Location = new System.Drawing.Point(264, 22);
            this.lable_sound_monster.Name = "lable_sound_monster";
            this.lable_sound_monster.Size = new System.Drawing.Size(50, 13);
            this.lable_sound_monster.TabIndex = 5;
            this.lable_sound_monster.Text = "Monsters";
            // 
            // trackbar_sound_monster
            // 
            this.trackbar_sound_monster.LargeChange = 10;
            this.trackbar_sound_monster.Location = new System.Drawing.Point(232, 38);
            this.trackbar_sound_monster.Maximum = 100;
            this.trackbar_sound_monster.Name = "trackbar_sound_monster";
            this.trackbar_sound_monster.Size = new System.Drawing.Size(104, 45);
            this.trackbar_sound_monster.SmallChange = 5;
            this.trackbar_sound_monster.TabIndex = 0;
            this.trackbar_sound_monster.TickFrequency = 20;
            this.trackbar_sound_monster.Scroll += new System.EventHandler(this.trackbar_sound_monster_Scroll);
            // 
            // label_sound_music
            // 
            this.label_sound_music.AutoSize = true;
            this.label_sound_music.Location = new System.Drawing.Point(163, 22);
            this.label_sound_music.Name = "label_sound_music";
            this.label_sound_music.Size = new System.Drawing.Size(35, 13);
            this.label_sound_music.TabIndex = 3;
            this.label_sound_music.Text = "Music";
            // 
            // trackbar_sound_music
            // 
            this.trackbar_sound_music.LargeChange = 10;
            this.trackbar_sound_music.Location = new System.Drawing.Point(122, 38);
            this.trackbar_sound_music.Maximum = 100;
            this.trackbar_sound_music.Name = "trackbar_sound_music";
            this.trackbar_sound_music.Size = new System.Drawing.Size(104, 45);
            this.trackbar_sound_music.SmallChange = 5;
            this.trackbar_sound_music.TabIndex = 0;
            this.trackbar_sound_music.TickFrequency = 20;
            this.trackbar_sound_music.Scroll += new System.EventHandler(this.trackbar_sound_music_Scroll);
            // 
            // label_sound_ambience
            // 
            this.label_sound_ambience.AutoSize = true;
            this.label_sound_ambience.Location = new System.Drawing.Point(37, 22);
            this.label_sound_ambience.Name = "label_sound_ambience";
            this.label_sound_ambience.Size = new System.Drawing.Size(54, 13);
            this.label_sound_ambience.TabIndex = 1;
            this.label_sound_ambience.Text = "Ambiance";
            // 
            // trackbar_sound_ambient
            // 
            this.trackbar_sound_ambient.LargeChange = 10;
            this.trackbar_sound_ambient.Location = new System.Drawing.Point(12, 38);
            this.trackbar_sound_ambient.Maximum = 100;
            this.trackbar_sound_ambient.Name = "trackbar_sound_ambient";
            this.trackbar_sound_ambient.Size = new System.Drawing.Size(104, 45);
            this.trackbar_sound_ambient.SmallChange = 5;
            this.trackbar_sound_ambient.TabIndex = 0;
            this.trackbar_sound_ambient.TickFrequency = 20;
            this.trackbar_sound_ambient.Scroll += new System.EventHandler(this.trackbar_sound_ambient_Scroll);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.ScreenSettingsGroup);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(456, 111);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // ScreenSettingsGroup
            // 
            this.ScreenSettingsGroup.Controls.Add(this.screenSettingsLabel);
            this.ScreenSettingsGroup.Controls.Add(this.heightLabel);
            this.ScreenSettingsGroup.Controls.Add(this.widthLabel);
            this.ScreenSettingsGroup.Controls.Add(this.fullscreenOptionsBox);
            this.ScreenSettingsGroup.Controls.Add(this.screenHBox);
            this.ScreenSettingsGroup.Controls.Add(this.screenWBox);
            this.ScreenSettingsGroup.Location = new System.Drawing.Point(3, 3);
            this.ScreenSettingsGroup.Name = "ScreenSettingsGroup";
            this.ScreenSettingsGroup.Size = new System.Drawing.Size(444, 96);
            this.ScreenSettingsGroup.TabIndex = 0;
            this.ScreenSettingsGroup.TabStop = false;
            this.ScreenSettingsGroup.Text = "Display Options";
            // 
            // screenSettingsLabel
            // 
            this.screenSettingsLabel.AutoSize = true;
            this.screenSettingsLabel.Location = new System.Drawing.Point(225, 25);
            this.screenSettingsLabel.Name = "screenSettingsLabel";
            this.screenSettingsLabel.Size = new System.Drawing.Size(111, 13);
            this.screenSettingsLabel.TabIndex = 5;
            this.screenSettingsLabel.Text = "Windowed/Fullscreen";
            // 
            // heightLabel
            // 
            this.heightLabel.AutoSize = true;
            this.heightLabel.Location = new System.Drawing.Point(118, 25);
            this.heightLabel.Name = "heightLabel";
            this.heightLabel.Size = new System.Drawing.Size(38, 13);
            this.heightLabel.TabIndex = 4;
            this.heightLabel.Text = "Height";
            // 
            // widthLabel
            // 
            this.widthLabel.AutoSize = true;
            this.widthLabel.Location = new System.Drawing.Point(9, 25);
            this.widthLabel.Name = "widthLabel";
            this.widthLabel.Size = new System.Drawing.Size(35, 13);
            this.widthLabel.TabIndex = 3;
            this.widthLabel.Text = "Width";
            // 
            // fullscreenOptionsBox
            // 
            this.fullscreenOptionsBox.FormattingEnabled = true;
            this.fullscreenOptionsBox.Items.AddRange(new object[] {
            "Windowed",
            "Borderless Fullscreen",
            "Fullscreen (Beware alerts and alt-tab!)"});
            this.fullscreenOptionsBox.Location = new System.Drawing.Point(225, 42);
            this.fullscreenOptionsBox.Name = "fullscreenOptionsBox";
            this.fullscreenOptionsBox.Size = new System.Drawing.Size(214, 21);
            this.fullscreenOptionsBox.TabIndex = 1;
            // 
            // screenHBox
            // 
            this.screenHBox.Location = new System.Drawing.Point(117, 42);
            this.screenHBox.Name = "screenHBox";
            this.screenHBox.Size = new System.Drawing.Size(100, 20);
            this.screenHBox.TabIndex = 1;
            this.screenHBox.Text = "1080";
            // 
            // screenWBox
            // 
            this.screenWBox.Location = new System.Drawing.Point(12, 42);
            this.screenWBox.Name = "screenWBox";
            this.screenWBox.Size = new System.Drawing.Size(100, 20);
            this.screenWBox.TabIndex = 0;
            this.screenWBox.Text = "1920";
            // 
            // Launcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 494);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Launcher";
            this.Text = "And Then it Ate You Launcher";
            this.Load += new System.EventHandler(this.Launcher_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.saveFileBox.ResumeLayout(false);
            this.difficultyBox.ResumeLayout(false);
            this.difficultyBox.PerformLayout();
            this.audioBox.ResumeLayout(false);
            this.audioBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackbar_sound_monster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackbar_sound_music)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackbar_sound_ambient)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ScreenSettingsGroup.ResumeLayout(false);
            this.ScreenSettingsGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button start_game_button;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox ScreenSettingsGroup;
        private System.Windows.Forms.ComboBox fullscreenOptionsBox;
        private System.Windows.Forms.TextBox screenHBox;
        private System.Windows.Forms.TextBox screenWBox;
        private System.Windows.Forms.Label widthLabel;
        private System.Windows.Forms.Label screenSettingsLabel;
        private System.Windows.Forms.Label heightLabel;
        private System.Windows.Forms.GroupBox audioBox;
        private System.Windows.Forms.TrackBar trackbar_sound_ambient;
        private System.Windows.Forms.Label label_sound_ambience;
        private System.Windows.Forms.TrackBar trackbar_sound_music;
        private System.Windows.Forms.Label label_sound_music;
        private System.Windows.Forms.Label lable_sound_monster;
        private System.Windows.Forms.TrackBar trackbar_sound_monster;
        private System.Windows.Forms.Button button_audio_test;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.GroupBox difficultyBox;
        private System.Windows.Forms.RadioButton difficulty_hard;
        private System.Windows.Forms.RadioButton difficulty_very_hard;
        private System.Windows.Forms.GroupBox saveFileBox;
        private System.Windows.Forms.ComboBox saveSlot;
    }
}