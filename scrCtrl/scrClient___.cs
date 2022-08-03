using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using WebSocketSharp;

namespace SCR.Controls
{
	class SCRClient
	{
		private string m_host { get; set; }
		public string Host { get { return m_host; } set { m_host = value; } }

		private int m_rport { get; set; }
		public int RestPort { get { return m_rport; } set { m_rport = value; } }

		private int m_wport { get; set; }
		public int WSockPort { get { return m_wport; } set { m_wport = value; } }

		private string api_key { get; set; }
		private int m_sendType { get; set; }


		private bool m_bLoop { get; set; }
		public bool LOOP
		{
			get
			{
				bool bLoop = false;
				Monitor.Enter(this);
				bLoop = m_bLoop;
				Monitor.Exit(this);
				return bLoop;
			}
			set
			{
				Monitor.Enter(this);
				m_bLoop = value;
				Monitor.Exit(this);
			}
		}
		private bool m_bConnectRA { get; set; }
		public bool CNCT_RA
		{
			get
			{
				bool b = false;
				Monitor.Enter(this);
				b = m_bConnectRA;
				Monitor.Exit(this);
				return b;
			}
			set
			{
				Monitor.Enter(this);
				m_bConnectRA = value;
				Monitor.Exit(this);
			}
		}
		private bool m_bConnectWS { get; set; }
		public bool CNCT_WS
		{
			get
			{
				bool b = false;
				Monitor.Enter(this);
				b = m_bConnectWS;
				Monitor.Exit(this);
				return b;
			}
			set
			{
				Monitor.Enter(this);
				m_bConnectWS = value;
				Monitor.Exit(this);
			}
		}
		WebSocket m_ws { get; set; }

		private string m_userid;
		private string m_userpw;

		public delegate void _event_msg(byte lv, string msg);
		public event _event_msg OnEventMsg;

		public delegate void _onreceive_msg(string msg);
		public event _onreceive_msg OnReceiveMsg;

		public delegate void _onreceive_bin(byte[] bin);
		public event _onreceive_bin OnReceiveBin;

		public SCRClient()
		{
		}


		public SCRClient(string ip, int rport, int wport)
		{
			Host = ip;
			WSockPort = wport;
			RestPort = rport;
			
			LOOP = CNCT_RA = CNCT_WS = false;
		}

		public SCRClient(string ip, int rport, int wport, string uid, string upw)
		{
			Host = ip;
			WSockPort = wport;
			RestPort = rport;
			m_userid = uid;
			m_userpw = upw;

			LOOP = CNCT_RA = CNCT_WS = false;
		}

		public void start()
		{
			LOOP = true;

			ThreadPool.QueueUserWorkItem(new WaitCallback(thread_restapi));
			ThreadPool.QueueUserWorkItem(new WaitCallback(thread_websocket));
		}
		public void start(string uid, string upw)
		{
			m_userid = uid;
			m_userpw = upw;

			start();
		}

		public void close()
		{
			if (m_ws != null)
				m_ws.Close();

			logout();

			LOOP = false;
			CNCT_RA = false;
			CNCT_WS = false;

			Monitor.Enter(this);
			api_key = "";
			Monitor.Exit(this);
		}

		private void login()
		{
			m_sendType = 1;
			request_data(Method.POST, "users/login", @"{""id"":""" + m_userid + @""", ""pw"":""" + m_userpw + @"""}");
		}

		private void logout()
		{
			if (CNCT_RA && !api_key.Equals(""))
			{
				m_sendType = 8;
				request_data(Method.DELETE, "users/logout", @"{""api-key"":""" + api_key + @"""}");
			}

			api_key = null;
			CNCT_RA = false;
		}

		private void keepalive()
		{
			if (api_key.IsNullOrEmpty())
				return;

			m_sendType = 9;
			request_data(Method.POST, "keepalive", @"{""api-key"":""" + api_key + @"""}");
		}

		private void get_camera_info()
		{
			m_sendType = 2;
			// ?get=id,pw,ip,port
			request_data(Method.POST, "devices/vaChannels", @"{""api-key"":""" + api_key + @"""}");
		}

		private void request_data(Method method, string api, string jdata)
		{
			if (!CNCT_RA && m_sendType != 1)
				return;

			try
			{
				Uri uri = new Uri("http://" + Host + ":" + RestPort.ToString());
				var client = new RestClient(uri);
				var request = new RestRequest(api, method, DataFormat.Json);
				request.AddHeader("cache-control", "no-cache");
				request.AddHeader("Content-Type", "application/json");
				request.AddJsonBody(jdata);

				var response = client.Execute(request);
				if (!response.IsSuccessful)
				{
					if(m_sendType == 1)
						OnEventMsg?.Invoke(0, string.Format("[RestAPI : {0}] 로그인을 하지 못했습니다.", Host));
					//OnEventMsg?.Invoke(0, "(Rest API) 로그인을 하지 못했습니다. 서버 확인 후 다시 실행 해주세요.");
					else if (m_sendType == 9)
						OnEventMsg?.Invoke(0, string.Format("[RestAPI : {0}] Keepalive 응답 오류", Host));

					/*						System.Diagnostics.Debug.WriteLine("=======================================");
											System.Diagnostics.Debug.WriteLine("[Keepalive 응답 오류]");
											System.Diagnostics.Debug.WriteLine("=======================================");
											System.Diagnostics.Debug.WriteLine(string.Format("Status Code : {0}", response.StatusCode));
											System.Diagnostics.Debug.WriteLine(string.Format("{0}", response.Content));
											System.Diagnostics.Debug.WriteLine(string.Format("URL : {0}, API : {1}, BODY : {2}, RESOURCE : {3}", ruri, api, jdata, response.Request.Resource));*/
					
					if (response.ErrorException != null)
						OnEventMsg?.Invoke(0, string.Format("[RestAPI : {0}] {1} - {2}", Host, response.ErrorException.Message, response.StatusDescription));

					if ((int)response.StatusCode == 451 || (int)response.StatusCode == 452)
					{
						OnEventMsg?.Invoke(0, string.Format("[RestAPI : {0}] {1} - {2}", Host, response.StatusCode, response.Content));
						Monitor.Enter(this);
						CNCT_WS = false;

						if (m_ws != null)
						{
							m_ws.Close();
							m_ws = null;
						}

						if (CNCT_RA)
							logout();
						Monitor.Exit(this);
					}

					return;
				}

				json_proc(response);
			}
			catch (Exception e)
			{
				CNCT_RA = false;
				OnEventMsg?.Invoke(0, "(Rest API2) " + e.Message);
			}
		}

		public void json_proc(IRestResponse response)
		{
			JsonSerializer json = new JsonSerializer();
			var jobj = json.Deserialize<Dictionary<string, string>>(response);

			if (m_sendType == 1)
			{
				string key = null;
				Monitor.Enter(this);
				key = api_key = jobj["api-key"].ToString();
				Monitor.Exit(this);

				if (key.IsNullOrEmpty())
				{
					OnEventMsg?.Invoke(1, string.Format("[RestAPI : {0}] API KEY 값 이상. 다시 로그인 합니다.", Host));
				}
				else
				{
					OnEventMsg?.Invoke(1, string.Format("[RestAPI : {0}] 로그인 하였습니다.", Host));
					CNCT_RA = true;
				}
			}
			else if (m_sendType == 2)
			{
				//OnCommReceive("N_CAMLIST", response.Content);
			}
			else if(m_sendType == 8)
			{
				close();
			}
			else
			{
				//OnEventMsg?.Invoke(response.Content);
			}
		}

		private void thread_restapi(object obj)
		{
			OnEventMsg?.Invoke(9, string.Format("[RestAPI - {0}] 스레드를 시작합니다.", Host));
			int nTM = 1;
			while (LOOP)
			{
				if (!CNCT_RA)
					login();

				if (nTM % 5 == 0 && CNCT_RA)
				{
					nTM = 1;
					keepalive();
				}

				Thread.Sleep(1000);
				nTM++;
			}
			OnEventMsg?.Invoke(9, string.Format("[RestAPI - {0}] 스레드를 종료합니다.", Host));
		}



		private void connect_websock()
		{
			OnEventMsg?.Invoke(1, string.Format("[WebSocket : {0}] 연결 하는 중입니다.", Host));
			if (CNCT_WS)
				m_ws.Close();

			m_ws.Connect();

			if (m_ws.IsAlive)
				LOOP = CNCT_WS = true;
			else
				OnEventMsg?.Invoke(1, string.Format("[WebSocket : {0}] 연결하지 못 했습니다.", Host));
		}

		private void OnWebSocket_Open(object sender, EventArgs e)
		{
			try
			{
				OnEventMsg?.Invoke(1, string.Format("[WebSocket : {0}] 연결 하였습니다.", Host));
				CNCT_WS = true;
			}
			catch (WebSocketException ex)
			{
				OnEventMsg?.Invoke(1, string.Format("[WebSocket : {0}] {1}", Host, ex.Message));
			}
		}

		private void OnWebSocket_Close(object sender, CloseEventArgs e)
		{
			try
			{
				OnEventMsg?.Invoke(1, string.Format("[WebSocket : {0}] 연결 종료합니다.", Host));

				Monitor.Enter(this);
				CNCT_WS = false;
				CNCT_RA = false;
				m_ws = null;
				api_key = null;
				Monitor.Exit(this);
			}
			catch (WebSocketException ex)
			{
				OnEventMsg?.Invoke(1, string.Format("[WebSocket : {0}] {1}", Host, ex.Message));
			}
		}

		private void OnWebSocket_Error(object sender, ErrorEventArgs e)
		{
			try
			{
				OnEventMsg?.Invoke(1, string.Format("[WebSocket : {0}] {1}", Host, e.Message));
			}
			catch (WebSocketException ex)
			{
				OnEventMsg?.Invoke(1, string.Format("[WebSocket : {0}] {1}", Host, ex.Message));
			}
		}

		private void OnWebSocket_MsgCompleted(object sender, MessageEventArgs e)
		{
			try
			{
				if (e.IsText)
				{
					OnReceiveMsg?.Invoke(e.Data);
					System.Diagnostics.Debug.WriteLine(e.Data);
				}

				if (e.IsBinary)
					OnReceiveBin?.Invoke(e.RawData);
			}
			catch(WebSocketException ex)
			{
				OnEventMsg?.Invoke(0, "(WebSocket) " + ex.Message);
			}
		}

		private void thread_websocket(object obj)
		{
			OnEventMsg?.Invoke(9, string.Format("[WebSocket - {0}] 스레드를 시작합니다.", Host));
			while (LOOP)
			{
				string skey = null;
				Monitor.Enter(this);
				skey = api_key;
				Monitor.Exit(this);

				if (skey != null && !skey.Equals(""))
				{
					if (m_ws == null)
					{
						m_ws = new WebSocket("ws://" + Host + ":" +
							WSockPort.ToString() + "/vaMetadata?api-key=" + skey +
							/*&frmMeta*/"&evtMeta=begun,ended,evtImg", "va-metadata");
						m_ws.OnOpen += OnWebSocket_Open;
						m_ws.OnClose += OnWebSocket_Close;
						m_ws.OnError += OnWebSocket_Error;
						m_ws.OnMessage += OnWebSocket_MsgCompleted;
					}

					if (!CNCT_WS)
						connect_websock();
				}

				Thread.Sleep(10);
			}
			OnEventMsg?.Invoke(9, string.Format("[WebSocket - {0}] 스레드를 종료합니다.", Host));
		}





		private int getPersonType(int ot)
		{
			int v = 20;
			if ((ot & 0x00000001) == 0)
				v = 10;

			if ((ot & 0x00000002) == 0)
				v += 2;
			else
				v += 1;

			return v;
		}
		private int getVehicleType(int ot)
		{
			int v = 0;
			if ((ot & 0x00000001) == 0)
				v = 30;
			else if ((ot & 0x00000002) == 0)
				v = 31;
			else if ((ot & 0x00000004) == 0)
				v = 32;
			else if ((ot & 0x00000008) == 0)
				v = 33;
			else if ((ot & 0x00000010) == 0)
				v = 34;
			else if ((ot & 0x00000020) == 0)
				v = 35;
			else if ((ot & 0x00000040) == 0)
				v = 36;

			return v;
		}
	}
}
