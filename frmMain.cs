using IntelliVIX_CM;
using LimeS.Log;
using SCR.Controls;
using System;
using System.Threading;
using System.Windows.Forms;

namespace SCR
{
	public partial class frmMain : Form
	{
		private LLog m_lLog { get; set; }
		private SCRClient[] m_scrClient { get; set; }

		private int m_nX { get; set; }
		private int m_nY { get; set; }

		private bool m_bTimer { get; set; }

		private int m_nCnt_R { get; set; }


		public frmMain()
        {
            InitializeComponent();
			m_nCnt_R = 0;
		}

        private void frmMain_Load(object sender, EventArgs e)
		{
			m_lLog = new LLog();
			writeLog(0, "프로세스 실행 중 입니다.");

			ThreadPool.QueueUserWorkItem(new WaitCallback(thread_init), this);
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			writeLog(0, "프로세스 종료대기 중 입니다.");

			for(int i = 0; i < m_scrClient.Length; i++)
				if(m_scrClient[i] != null)
					m_scrClient[i].close();

			ThreadPool.QueueUserWorkItem(new WaitCallback(thread_chk_close), 3);
		}

		private void form_close()
		{
			try
			{
				if (InvokeRequired)
				{
					Invoke(new MethodInvoker(delegate () { form_close(); }));
				}
				else
				{
					//Monitor.Enter(this);
					m_bTimer = false;
					//Monitor.Exit(this);

					writeLog(0, "프로세스를 종료하였습니다.");
					// 파일 쓰기 스레드 종료처리
					m_lLog.LOOP = false;
					thread_chk_close(1);

					Close();
				}
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine(e.ToString());
			}
		}

		/// <summary>
		/// 로그 파일 작성하기
		/// </summary>
		/// <param name="lv">메시지 등급 레벨 0~9</param>
		/// <param name="msg">메시지</param>
		public void writeLog(byte lv, string msg)
		{
			if (lvLog.InvokeRequired)
			{
				Invoke(new MethodInvoker(delegate () { writeLog(lv, msg); }), new object[] { lv, msg });
			}
			else
			{
				if (m_lLog != null)
					m_lLog.add_log(lv, msg);

				if (lvLog.Items.Count > 1000)
					lvLog.Items.RemoveAt(0);

				msg = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "] " + msg;
				lvLog.Items.Add(msg);
				lvLog.TopIndex = lvLog.Items.Count - 1;
			}
		}
		
		private void redraw_progressbar()
		{
			if (InvokeRequired)
			{
				Invoke(new MethodInvoker(delegate () { redraw_progressbar(); }));
			}
			else
			{
				pgbRecv.Value = m_nCnt_R > pgbRecv.Maximum ? pgbRecv.Maximum : m_nCnt_R;
				lbRCnt.Text = m_nCnt_R.ToString();
				m_nCnt_R = 0;
			}
		}

		private void plTop_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				m_nX = e.X;
				m_nY = e.Y;
			}
		}

		private void plTop_MouseMove(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left)
			{
				int nX = Left - (m_nX - e.X);
				int nY = Top - (m_nY - e.Y);

				Location = new System.Drawing.Point(nX, nY);
			}
		}

		private void OnReceiveMsg(string m)
		{
			try
			{
				m_nCnt_R++;
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine(e.ToString());
			}
		}
		private void OnReceiveBin(byte[] b)
		{
			try
			{
				//if (m_procData != null)
				//	m_procData.add_data(b);

				m_nCnt_R++;
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine(e.ToString());
			}
		}

		private void thread_chk_close(object obj)
		{
			System.Diagnostics.Debug.WriteLine("[thread_chk_close] start");
			// 모든 쓰레드 종료 대기 
			int workerThreads = 0;
			int completionPortThread = 0;
			int maxWorkerThreads = 0;
			int maxCompletionPortThreads = 0;

			ThreadPool.GetMaxThreads(out maxWorkerThreads, out maxCompletionPortThreads);

			while (true)
			{
				ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThread);

				if (workerThreads + (int)obj >= maxWorkerThreads)
					break;

				Thread.Sleep(100);
			}

			if((int)obj != 1)
				form_close();
			System.Diagnostics.Debug.WriteLine("[thread_chk_close] end");
		}

		private void thread_timer(object obj)
		{
			System.Diagnostics.Debug.WriteLine("[thread_timer] start");
			m_bTimer = true;

			while (m_bTimer)
			{
				// 1초 주기로 프로세스바 갱신
				redraw_progressbar();
				Thread.Sleep(1000);
			}

			//redraw_progressbar();
			System.Diagnostics.Debug.WriteLine("[thread_timer] end");
		}

		private void thread_init(object obj)
		{
			byte logLv = AppConfig.GetAppConfig("Log Level") == null ? (byte)0x0 : byte.Parse(AppConfig.GetAppConfig("Log Level"));
			string uid = AppConfig.GetAppConfig("Server ID") == null ? "IntelliVIX" : AppConfig.GetAppConfig("Server ID");
			string upw = AppConfig.GetAppConfig("Server PW") == null ? "pass0001!" : AppConfig.GetAppConfig("Server PW");
			int scnt = AppConfig.GetAppConfig("Server Count") == null ? 1 : int.Parse(AppConfig.GetAppConfig("Server Count"));

			m_lLog.set_logLevel(logLv);

			m_scrClient = new SCRClient[scnt];
			for (int i = 0; i < scnt; i++)
			{
				string _i = (i + 1).ToString();
				string ip = AppConfig.GetAppConfig("Server HOST" + _i) == null ? "intellivix.iptime.org" : AppConfig.GetAppConfig("Server HOST" + _i);
				int rport = AppConfig.GetAppConfig("Rest API PORT" + _i) == null ? 17681 : int.Parse(AppConfig.GetAppConfig("Rest API PORT" + _i));
				int wport = AppConfig.GetAppConfig("WebSock PORT" + _i) == null ? 17681 : int.Parse(AppConfig.GetAppConfig("WebSock PORT" + _i));

				m_scrClient[i] = new SCRClient(ip, rport, wport);
				m_scrClient[i].OnEventMsg += new SCRClient._event_msg(writeLog);
				m_scrClient[i].OnReceiveMsg += new SCRClient._onreceive_msg(OnReceiveMsg);
				m_scrClient[i].OnReceiveBin += new SCRClient._onreceive_bin(OnReceiveBin);
				m_scrClient[i].start(uid, upw);
			}

			ThreadPool.QueueUserWorkItem(new WaitCallback(thread_timer), this);
			writeLog(0, "프로세스 실행을 완료 하였습니다.");
		}
	}
}
