using System;
using System.Windows.Forms;

namespace SCR
{
	static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
			// 현재 실행한 프로그램 ID
			int nPid = System.Diagnostics.Process.GetCurrentProcess().Id;

			// 실행중인 프로세스 정보 수집
			System.Diagnostics.Process[] p = System.Diagnostics.Process.GetProcessesByName("IntelliVIX 연동서버");

			if (p.Length > 1)
			{
				for (int i = 0; i < p.Length; i++)
				{
					if (p[i].Id == nPid)
					{
						MessageBox.Show(
							"이미 실행 중입니다.",
							"확인",
							MessageBoxButtons.OK,
							MessageBoxIcon.Warning,
							MessageBoxDefaultButton.Button1
						);

						Application.Exit();
					}
				}
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new frmMain());
		}
    }
}
