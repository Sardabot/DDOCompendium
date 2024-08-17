namespace DDOCompendium
{
    partial class Main
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.datagridQuests = new System.Windows.Forms.DataGridView();
            this.tcTabs = new System.Windows.Forms.TabControl();
            this.tabNotes = new System.Windows.Forms.TabPage();
            this.tabNotes2 = new System.Windows.Forms.TabPage();
            this.tabRefTables = new System.Windows.Forms.TabPage();
            this.tabWilderness = new System.Windows.Forms.TabPage();
            this.tabCharacters = new System.Windows.Forms.TabPage();
            this.tabSagas = new System.Windows.Forms.TabPage();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.contextmenuQuestCompletion = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsitemCasual = new System.Windows.Forms.ToolStripMenuItem();
            this.tsitemNormal = new System.Windows.Forms.ToolStripMenuItem();
            this.tsitemHard = new System.Windows.Forms.ToolStripMenuItem();
            this.tsitemElite = new System.Windows.Forms.ToolStripMenuItem();
            this.tsitemClear = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datagridQuests)).BeginInit();
            this.tcTabs.SuspendLayout();
            this.contextmenuQuestCompletion.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.datagridQuests);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tcTabs);
            this.splitContainer1.Size = new System.Drawing.Size(1233, 616);
            this.splitContainer1.SplitterDistance = 716;
            this.splitContainer1.TabIndex = 0;
            // 
            // datagridQuests
            // 
            this.datagridQuests.AllowUserToAddRows = false;
            this.datagridQuests.AllowUserToDeleteRows = false;
            this.datagridQuests.AllowUserToResizeRows = false;
            this.datagridQuests.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.datagridQuests.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.datagridQuests.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datagridQuests.Location = new System.Drawing.Point(0, 0);
            this.datagridQuests.Name = "datagridQuests";
            this.datagridQuests.ReadOnly = true;
            this.datagridQuests.Size = new System.Drawing.Size(716, 616);
            this.datagridQuests.TabIndex = 0;
            this.datagridQuests.CellContextMenuStripNeeded += new System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventHandler(this.DatagridQuests_CellContextMenuStripNeeded);
            this.datagridQuests.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DatagridQuests_CellMouseClick);
            // 
            // tcTabs
            // 
            this.tcTabs.Controls.Add(this.tabNotes);
            this.tcTabs.Controls.Add(this.tabNotes2);
            this.tcTabs.Controls.Add(this.tabRefTables);
            this.tcTabs.Controls.Add(this.tabWilderness);
            this.tcTabs.Controls.Add(this.tabCharacters);
            this.tcTabs.Controls.Add(this.tabSagas);
            this.tcTabs.Controls.Add(this.tabSettings);
            this.tcTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcTabs.Location = new System.Drawing.Point(0, 0);
            this.tcTabs.Name = "tcTabs";
            this.tcTabs.SelectedIndex = 0;
            this.tcTabs.Size = new System.Drawing.Size(513, 616);
            this.tcTabs.TabIndex = 0;
            // 
            // tabNotes
            // 
            this.tabNotes.Location = new System.Drawing.Point(4, 22);
            this.tabNotes.Name = "tabNotes";
            this.tabNotes.Padding = new System.Windows.Forms.Padding(3);
            this.tabNotes.Size = new System.Drawing.Size(505, 590);
            this.tabNotes.TabIndex = 0;
            this.tabNotes.Text = "Notes";
            this.tabNotes.UseVisualStyleBackColor = true;
            // 
            // tabNotes2
            // 
            this.tabNotes2.Location = new System.Drawing.Point(4, 22);
            this.tabNotes2.Name = "tabNotes2";
            this.tabNotes2.Padding = new System.Windows.Forms.Padding(3);
            this.tabNotes2.Size = new System.Drawing.Size(505, 590);
            this.tabNotes2.TabIndex = 1;
            this.tabNotes2.Text = "Notes 2";
            this.tabNotes2.UseVisualStyleBackColor = true;
            // 
            // tabRefTables
            // 
            this.tabRefTables.Location = new System.Drawing.Point(4, 22);
            this.tabRefTables.Name = "tabRefTables";
            this.tabRefTables.Size = new System.Drawing.Size(505, 590);
            this.tabRefTables.TabIndex = 2;
            this.tabRefTables.Text = "Ref Tables";
            this.tabRefTables.UseVisualStyleBackColor = true;
            // 
            // tabWilderness
            // 
            this.tabWilderness.Location = new System.Drawing.Point(4, 22);
            this.tabWilderness.Name = "tabWilderness";
            this.tabWilderness.Size = new System.Drawing.Size(505, 590);
            this.tabWilderness.TabIndex = 3;
            this.tabWilderness.Text = "Wilderness";
            this.tabWilderness.UseVisualStyleBackColor = true;
            // 
            // tabCharacters
            // 
            this.tabCharacters.Location = new System.Drawing.Point(4, 22);
            this.tabCharacters.Name = "tabCharacters";
            this.tabCharacters.Size = new System.Drawing.Size(505, 590);
            this.tabCharacters.TabIndex = 4;
            this.tabCharacters.Text = "Characters";
            this.tabCharacters.UseVisualStyleBackColor = true;
            // 
            // tabSagas
            // 
            this.tabSagas.Location = new System.Drawing.Point(4, 22);
            this.tabSagas.Name = "tabSagas";
            this.tabSagas.Size = new System.Drawing.Size(505, 590);
            this.tabSagas.TabIndex = 5;
            this.tabSagas.Text = "Sagas";
            this.tabSagas.UseVisualStyleBackColor = true;
            // 
            // tabSettings
            // 
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabSettings.Size = new System.Drawing.Size(505, 590);
            this.tabSettings.TabIndex = 6;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // contextmenuQuestCompletion
            // 
            this.contextmenuQuestCompletion.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsitemCasual,
            this.tsitemNormal,
            this.tsitemHard,
            this.tsitemElite,
            this.tsitemClear});
            this.contextmenuQuestCompletion.Name = "contextmenuQuestCompletion";
            this.contextmenuQuestCompletion.Size = new System.Drawing.Size(181, 136);
            this.contextmenuQuestCompletion.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ContextmenuQuestCompletion_ItemClicked);
            // 
            // tsitemCasual
            // 
            this.tsitemCasual.Name = "tsitemCasual";
            this.tsitemCasual.Size = new System.Drawing.Size(180, 22);
            this.tsitemCasual.Text = "Casual";
            // 
            // tsitemNormal
            // 
            this.tsitemNormal.Name = "tsitemNormal";
            this.tsitemNormal.Size = new System.Drawing.Size(180, 22);
            this.tsitemNormal.Text = "Normal";
            // 
            // tsitemHard
            // 
            this.tsitemHard.Name = "tsitemHard";
            this.tsitemHard.Size = new System.Drawing.Size(180, 22);
            this.tsitemHard.Text = "Hard";
            // 
            // tsitemElite
            // 
            this.tsitemElite.Name = "tsitemElite";
            this.tsitemElite.Size = new System.Drawing.Size(180, 22);
            this.tsitemElite.Text = "Elite";
            // 
            // tsitemClear
            // 
            this.tsitemClear.Name = "tsitemClear";
            this.tsitemClear.Size = new System.Drawing.Size(180, 22);
            this.tsitemClear.Text = "Clear";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1233, 616);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Main";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.datagridQuests)).EndInit();
            this.tcTabs.ResumeLayout(false);
            this.contextmenuQuestCompletion.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView datagridQuests;
        private System.Windows.Forms.TabControl tcTabs;
        private System.Windows.Forms.TabPage tabNotes;
        private System.Windows.Forms.TabPage tabNotes2;
        private System.Windows.Forms.TabPage tabRefTables;
        private System.Windows.Forms.TabPage tabWilderness;
        private System.Windows.Forms.TabPage tabCharacters;
        private System.Windows.Forms.TabPage tabSagas;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.ContextMenuStrip contextmenuQuestCompletion;
        private System.Windows.Forms.ToolStripMenuItem tsitemCasual;
        private System.Windows.Forms.ToolStripMenuItem tsitemNormal;
        private System.Windows.Forms.ToolStripMenuItem tsitemHard;
        private System.Windows.Forms.ToolStripMenuItem tsitemElite;
        private System.Windows.Forms.ToolStripMenuItem tsitemClear;
    }
}

