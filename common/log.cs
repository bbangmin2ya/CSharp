using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace LimeS.Log
{
    class LLog
	{
		private string m_path { get; set; }
		private string m_fn { get; set; }	// 파일명 (전체 파일명 = 파일명 + 시간.확장자)
		private string m_en { get; set; }	// 확장자명

		private bool m_bLoop { get; set; }
		public bool LOOP { get { return m_bLoop; } set { m_bLoop = value; } }

		private byte m_nLv;

		List<string> m_list { get; set; }
		public int COUNT { get { Monitor.Enter(this); int cnt = m_list.Count; Monitor.Exit(this); return cnt; } }


		public LLog(string path = "\\log", string fn = "", string extnm = "log")
		{
			m_bLoop = true;
			m_path = Application.StartupPath + path;
			m_list = new List<string>();

			if (!Directory.Exists(m_path))
				Directory.CreateDirectory(m_path);

			m_fn = fn;
			m_en = extnm;

			ThreadPool.QueueUserWorkItem(new WaitCallback(thread_proc));
		}

		public void add_log(byte lv,  string msg, bool time = true)
		{
			if (m_nLv < lv)
				return;

			if (LOOP)
			{
				if (time)
					msg = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "] " + msg;
				Monitor.Enter(this);
				m_list.Add(msg);
				Monitor.Exit(this);
			}
		}

		public void set_logLevel(byte lv)
		{
			m_nLv = lv;
		}

		private void write_log(string msg)
		{
			string path = m_path + "\\" + DateTime.Now.ToString("yyyyMMdd");
			string fn = path + "\\" + m_fn + DateTime.Now.ToString("yyyyMMddHH") + "." + m_en;

			try
			{
				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);

				FileStream fs = new FileStream(fn, FileMode.Append);
				StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);

				sw.WriteLine(msg);
				sw.Close();
				//fs.Close();
			}
			catch(ObjectDisposedException ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}

		private bool delete_logfile(DateTime dtm)
		{
			dtm.AddDays(-7);
			string strdtm = dtm.ToString("yyyyMMdd");
			string path = m_path + "\\" + strdtm;

			try
			{
				string[] dirs = Directory.GetDirectories(m_path);

				for (int i = 0; i < dirs.Length; i++)
				{
					if(dirs[i].CompareTo(strdtm) <= 0)
					{
						DirectoryInfo drct = new DirectoryInfo(m_path + "\\" + dirs[i]);

						if (drct != null)
							drct.Delete(true);
					}
				}
			}
			catch (ObjectDisposedException ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				return false;
			}

			return true;
		}

		private void thread_proc(object obj)
		{
			if(m_fn.Equals(""))
				write_log("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "] 로그 쓰기를 시작합니다.");

			bool bDel = false;

			while (true)
			{
				// 이전 로그 삭제 추가 필요
				DateTime dtm = DateTime.Now;

				if (dtm.Hour == 0 && dtm.Minute == 0 && dtm.Second == 0 && !bDel)
					bDel = delete_logfile(dtm);

				if (dtm.Hour == 23 && dtm.Minute == 59 && dtm.Second == 0 && bDel)
					bDel = false;

				if (COUNT > 0)
				{
					Monitor.Enter(this);
					string msg = m_list[0];
					m_list.RemoveAt(0);
					Monitor.Exit(this);

					write_log(msg);
				}

				Thread.Sleep(1);

				if (!LOOP && COUNT == 0)
					break;
			}

			if (m_fn.Equals(""))
				write_log("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "] 로그 쓰기를 종료합니다.");
			System.Diagnostics.Debug.WriteLine("[thread_logproc] end");
		}
	}
}
