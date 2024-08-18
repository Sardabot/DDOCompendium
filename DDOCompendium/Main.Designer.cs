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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.datagridQuests = new System.Windows.Forms.DataGridView();
            this.tcTabs = new System.Windows.Forms.TabControl();
            this.tabNotes = new System.Windows.Forms.TabPage();
            this.txtNotes1 = new System.Windows.Forms.TextBox();
            this.tabNotes2 = new System.Windows.Forms.TabPage();
            this.txtNotes2 = new System.Windows.Forms.TextBox();
            this.tabRefTables = new System.Windows.Forms.TabPage();
            this.tabWilderness = new System.Windows.Forms.TabPage();
            this.tabCharacters = new System.Windows.Forms.TabPage();
            this.tabSagas = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelSagas = new System.Windows.Forms.TableLayoutPanel();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.contextmenuQuestCompletion = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsitemCasual = new System.Windows.Forms.ToolStripMenuItem();
            this.tsitemNormal = new System.Windows.Forms.ToolStripMenuItem();
            this.tsitemHard = new System.Windows.Forms.ToolStripMenuItem();
            this.tsitemElite = new System.Windows.Forms.ToolStripMenuItem();
            this.tsitemClear = new System.Windows.Forms.ToolStripMenuItem();
            this.contextmenuCharSelect = new System.Windows.Forms.ContextMenuStrip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datagridQuests)).BeginInit();
            this.tcTabs.SuspendLayout();
            this.tabNotes.SuspendLayout();
            this.tabNotes2.SuspendLayout();
            this.tabSagas.SuspendLayout();
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
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.Controls.Add(this.datagridQuests);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
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
            this.datagridQuests.BackgroundColor = System.Drawing.SystemColors.ControlDarkDark;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.datagridQuests.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.datagridQuests.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.datagridQuests.DefaultCellStyle = dataGridViewCellStyle2;
            this.datagridQuests.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datagridQuests.EnableHeadersVisualStyles = false;
            this.datagridQuests.Location = new System.Drawing.Point(0, 0);
            this.datagridQuests.Name = "datagridQuests";
            this.datagridQuests.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.datagridQuests.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.datagridQuests.RowHeadersVisible = false;
            this.datagridQuests.Size = new System.Drawing.Size(716, 616);
            this.datagridQuests.TabIndex = 0;
            this.datagridQuests.CellContextMenuStripNeeded += new System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventHandler(this.DatagridQuests_CellContextMenuStripNeeded);
            this.datagridQuests.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DatagridQuests_CellMouseClick);
            // 
            // tcTabs
            // 
            this.tcTabs.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
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
            this.tcTabs.SelectedIndexChanged += new System.EventHandler(this.TcTabs_SelectedIndexChanged);
            // 
            // tabNotes
            // 
            this.tabNotes.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.tabNotes.Controls.Add(this.txtNotes1);
            this.tabNotes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tabNotes.Location = new System.Drawing.Point(4, 25);
            this.tabNotes.Name = "tabNotes";
            this.tabNotes.Padding = new System.Windows.Forms.Padding(3);
            this.tabNotes.Size = new System.Drawing.Size(505, 587);
            this.tabNotes.TabIndex = 0;
            this.tabNotes.Text = "Notes";
            // 
            // txtNotes1
            // 
            this.txtNotes1.AcceptsReturn = true;
            this.txtNotes1.AcceptsTab = true;
            this.txtNotes1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtNotes1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNotes1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNotes1.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.txtNotes1.Location = new System.Drawing.Point(3, 3);
            this.txtNotes1.Multiline = true;
            this.txtNotes1.Name = "txtNotes1";
            this.txtNotes1.Size = new System.Drawing.Size(499, 581);
            this.txtNotes1.TabIndex = 0;
            // 
            // tabNotes2
            // 
            this.tabNotes2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.tabNotes2.Controls.Add(this.txtNotes2);
            this.tabNotes2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tabNotes2.Location = new System.Drawing.Point(4, 25);
            this.tabNotes2.Name = "tabNotes2";
            this.tabNotes2.Padding = new System.Windows.Forms.Padding(3);
            this.tabNotes2.Size = new System.Drawing.Size(505, 587);
            this.tabNotes2.TabIndex = 1;
            this.tabNotes2.Text = "Notes 2";
            // 
            // txtNotes2
            // 
            this.txtNotes2.AcceptsReturn = true;
            this.txtNotes2.AcceptsTab = true;
            this.txtNotes2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtNotes2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNotes2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNotes2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtNotes2.Location = new System.Drawing.Point(3, 3);
            this.txtNotes2.Multiline = true;
            this.txtNotes2.Name = "txtNotes2";
            this.txtNotes2.Size = new System.Drawing.Size(499, 581);
            this.txtNotes2.TabIndex = 0;
            // 
            // tabRefTables
            // 
            this.tabRefTables.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.tabRefTables.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tabRefTables.Location = new System.Drawing.Point(4, 25);
            this.tabRefTables.Name = "tabRefTables";
            this.tabRefTables.Size = new System.Drawing.Size(505, 587);
            this.tabRefTables.TabIndex = 2;
            this.tabRefTables.Text = "Ref Tables";
            // 
            // tabWilderness
            // 
            this.tabWilderness.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.tabWilderness.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tabWilderness.Location = new System.Drawing.Point(4, 25);
            this.tabWilderness.Name = "tabWilderness";
            this.tabWilderness.Size = new System.Drawing.Size(505, 587);
            this.tabWilderness.TabIndex = 3;
            this.tabWilderness.Text = "Wilderness";
            // 
            // tabCharacters
            // 
            this.tabCharacters.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.tabCharacters.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tabCharacters.Location = new System.Drawing.Point(4, 25);
            this.tabCharacters.Name = "tabCharacters";
            this.tabCharacters.Size = new System.Drawing.Size(505, 587);
            this.tabCharacters.TabIndex = 4;
            this.tabCharacters.Text = "Characters";
            // 
            // tabSagas
            // 
            this.tabSagas.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.tabSagas.Controls.Add(this.tableLayoutPanelSagas);
            this.tabSagas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tabSagas.Location = new System.Drawing.Point(4, 25);
            this.tabSagas.Name = "tabSagas";
            this.tabSagas.Size = new System.Drawing.Size(505, 587);
            this.tabSagas.TabIndex = 5;
            this.tabSagas.Text = "Sagas";
            // 
            // tableLayoutPanelSagas
            // 
            this.tableLayoutPanelSagas.AutoScroll = true;
            this.tableLayoutPanelSagas.AutoSize = true;
            this.tableLayoutPanelSagas.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelSagas.ColumnCount = 1;
            this.tableLayoutPanelSagas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSagas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSagas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSagas.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelSagas.Name = "tableLayoutPanelSagas";
            this.tableLayoutPanelSagas.RowCount = 1;
            this.tableLayoutPanelSagas.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSagas.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSagas.Size = new System.Drawing.Size(505, 587);
            this.tableLayoutPanelSagas.TabIndex = 0;
            // 
            // tabSettings
            // 
            this.tabSettings.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.tabSettings.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tabSettings.Location = new System.Drawing.Point(4, 25);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabSettings.Size = new System.Drawing.Size(505, 587);
            this.tabSettings.TabIndex = 6;
            this.tabSettings.Text = "Settings";
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
            this.contextmenuQuestCompletion.Size = new System.Drawing.Size(115, 114);
            this.contextmenuQuestCompletion.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ContextmenuQuestCompletion_ItemClicked);
            // 
            // tsitemCasual
            // 
            this.tsitemCasual.Name = "tsitemCasual";
            this.tsitemCasual.Size = new System.Drawing.Size(114, 22);
            this.tsitemCasual.Text = "Casual";
            // 
            // tsitemNormal
            // 
            this.tsitemNormal.Name = "tsitemNormal";
            this.tsitemNormal.Size = new System.Drawing.Size(114, 22);
            this.tsitemNormal.Text = "Normal";
            // 
            // tsitemHard
            // 
            this.tsitemHard.Name = "tsitemHard";
            this.tsitemHard.Size = new System.Drawing.Size(114, 22);
            this.tsitemHard.Text = "Hard";
            // 
            // tsitemElite
            // 
            this.tsitemElite.Name = "tsitemElite";
            this.tsitemElite.Size = new System.Drawing.Size(114, 22);
            this.tsitemElite.Text = "Elite";
            // 
            // tsitemClear
            // 
            this.tsitemClear.Name = "tsitemClear";
            this.tsitemClear.Size = new System.Drawing.Size(114, 22);
            this.tsitemClear.Text = "Clear";
            // 
            // contextmenuCharSelect
            // 
            this.contextmenuCharSelect.Name = "contextmenuCharSelect";
            this.contextmenuCharSelect.Size = new System.Drawing.Size(61, 4);
            this.contextmenuCharSelect.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextmenuCharSelect_ItemClicked);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(1233, 616);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Main";
            this.Text = "DDO Compendium";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.datagridQuests)).EndInit();
            this.tcTabs.ResumeLayout(false);
            this.tabNotes.ResumeLayout(false);
            this.tabNotes.PerformLayout();
            this.tabNotes2.ResumeLayout(false);
            this.tabNotes2.PerformLayout();
            this.tabSagas.ResumeLayout(false);
            this.tabSagas.PerformLayout();
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
        private System.Windows.Forms.TextBox txtNotes1;
        private System.Windows.Forms.TextBox txtNotes2;
        private System.Windows.Forms.ContextMenuStrip contextmenuCharSelect;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSagas;
    }
}

