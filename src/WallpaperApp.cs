using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace WallpaperTool
{
    public class MainForm : Form
    {
        private TextBox txtDestDir;
        private Button btnBrowse;
        private TextBox txtUrl;
        private TextBox txtLogFile;
        private Button btnDownload;
        private TextBox txtLog;
        private Label lblStatus;
        
        // Default values
        private string defaultUrl = "https://picsum.photos/1920/1080";
        private string defaultLogFile = "wallpaper_log.txt";

        public MainForm()
        {
            InitializeComponent();
            LoadDefaultSettings();
        }

        private void InitializeComponent()
        {
            this.Text = "壁纸下载工具";
            this.Size = new Size(500, 500); // Increased height to fit new controls
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Save Directory
            Label lblDir = new Label();
            lblDir.Text = "保存目录:";
            lblDir.Location = new Point(20, 20);
            lblDir.AutoSize = true;
            this.Controls.Add(lblDir);

            txtDestDir = new TextBox();
            txtDestDir.Location = new Point(20, 45);
            txtDestDir.Size = new Size(350, 25);
            this.Controls.Add(txtDestDir);

            btnBrowse = new Button();
            btnBrowse.Text = "浏览";
            btnBrowse.Location = new Point(380, 44);
            btnBrowse.Size = new Size(80, 27);
            btnBrowse.Click += BtnBrowse_Click;
            this.Controls.Add(btnBrowse);

            // Wallpaper URL
            Label lblUrl = new Label();
            lblUrl.Text = "壁纸下载地址 (URL):";
            lblUrl.Location = new Point(20, 80);
            lblUrl.AutoSize = true;
            this.Controls.Add(lblUrl);

            txtUrl = new TextBox();
            txtUrl.Location = new Point(20, 105);
            txtUrl.Size = new Size(440, 25);
            this.Controls.Add(txtUrl);

            // Log Filename
            Label lblLogFile = new Label();
            lblLogFile.Text = "日志文件名:";
            lblLogFile.Location = new Point(20, 140);
            lblLogFile.AutoSize = true;
            this.Controls.Add(lblLogFile);

            txtLogFile = new TextBox();
            txtLogFile.Location = new Point(20, 165);
            txtLogFile.Size = new Size(440, 25);
            this.Controls.Add(txtLogFile);

            // Download Button
            btnDownload = new Button();
            btnDownload.Text = "立即下载壁纸";
            btnDownload.Location = new Point(20, 210);
            btnDownload.Size = new Size(440, 40);
            btnDownload.Font = new Font(this.Font, FontStyle.Bold);
            btnDownload.Click += BtnDownload_Click;
            this.Controls.Add(btnDownload);

            // Log Output
            Label lblLog = new Label();
            lblLog.Text = "运行日志:";
            lblLog.Location = new Point(20, 260);
            lblLog.AutoSize = true;
            this.Controls.Add(lblLog);

            txtLog = new TextBox();
            txtLog.Location = new Point(20, 285);
            txtLog.Size = new Size(440, 130);
            txtLog.Multiline = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.ReadOnly = true;
            this.Controls.Add(txtLog);

            // Status Bar
            lblStatus = new Label();
            lblStatus.Text = "就绪";
            lblStatus.Location = new Point(20, 425);
            lblStatus.AutoSize = true;
            this.Controls.Add(lblStatus);
        }

        private void LoadDefaultSettings()
        {
            string picturesPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            string savedPictures = Path.Combine(picturesPath, "Saved Pictures");
            txtDestDir.Text = savedPictures;
            
            // Set default values for URL and Log File
            txtUrl.Text = defaultUrl;
            txtLogFile.Text = defaultLogFile;
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.SelectedPath = txtDestDir.Text;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtDestDir.Text = fbd.SelectedPath;
                }
            }
        }

        private void BtnDownload_Click(object sender, EventArgs e)
        {
            btnDownload.Enabled = false;
            lblStatus.Text = "正在下载...";
            Log("开始下载...");

            string destDir = txtDestDir.Text;
            string destFile = Path.Combine(destDir, "wallpaper_Terminal.jpg");
            string downloadUrl = txtUrl.Text.Trim();

            if (string.IsNullOrEmpty(downloadUrl))
            {
                MessageBox.Show("请输入有效的壁纸下载地址。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnDownload.Enabled = true;
                return;
            }

            try
            {
                if (!Directory.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                }

                using (WebClient client = new WebClient())
                {
                    // Use standard TLS protocols
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    
                    client.DownloadFileCompleted += (s, args) =>
                    {
                        if (args.Error != null)
                        {
                            HandleError(args.Error);
                        }
                        else
                        {
                            string msg = string.Format("成功: 壁纸已更新至 {0}", destFile);
                            Log(msg);
                            lblStatus.Text = "完成!";
                            MessageBox.Show("壁纸下载成功!", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        btnDownload.Enabled = true;
                    };

                    client.DownloadFileAsync(new Uri(downloadUrl), destFile);
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
                btnDownload.Enabled = true;
            }
        }

        private void HandleError(Exception ex)
        {
            string errorMsg = string.Format("错误: {0}", ex.Message);
            Log(errorMsg);
            lblStatus.Text = "错误";
            MessageBox.Show(errorMsg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void Log(string message)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logEntry = string.Format("[{0}] {1}", date, message);
            
            // UI Update
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() => txtLog.AppendText(logEntry + Environment.NewLine)));
            }
            else
            {
                txtLog.AppendText(logEntry + Environment.NewLine);
            }

            // File Logging
            try
            {
                string logFileName = txtLogFile.Text.Trim();
                if (string.IsNullOrEmpty(logFileName))
                {
                    logFileName = defaultLogFile;
                }
                
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logFileName);
                File.AppendAllText(logPath, logEntry + Environment.NewLine);
            }
            catch { /* Ignore log file errors */ }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
