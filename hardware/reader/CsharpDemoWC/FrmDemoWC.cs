using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO.Ports;
using System.Data.OleDb;

namespace CsharpDemoWC
{
    public partial class FrmDemoWC : Form
    {
        Reader Reader = new Reader();

        public UInt16 res;
        public int m_hScanner = -1, _OK = 0;
        public int ComMode = 0;//0--TCP/IP, 1--RS232,2--RS485, 3--USB
        //输入参数:	lOpt 为何种方式通信(0--TCP/IP, 1--RS232,2--RS485, 3--USB)
        public long lopt = 0;
        public string szPort;
        public byte connect_OK = 0;////0--no connect, 1--connected
        public int HardVersion, SoftVersion;
        int RS485Address = 0;

        //基本参数和自动参数
        public Reader.ReaderBasicParam gBasicParam = new Reader.ReaderBasicParam();
        public Reader.ReaderAutoParam gAutoParam = new Reader.ReaderAutoParam();
        public Reader.SimParam Param = new Reader.SimParam();//自模下仿真参数

        byte gFre = 0;	//取哪个国家的频率

        public byte[] gReaderID = new byte[33];			//读写器ID

        public Reader.tagReaderFreq[] stuctFreqCountry = new Reader.tagReaderFreq[25];

        //================ISO 18000 6C============================================
        int m_Word = 0;//数据位数
        int m_WriteMode = 0; //选中EPC---0, 选中用户区---1, 选中密码区---2
        int iFlagProtectOpt;//0---不保护(不锁), 1---使用密码锁，2---永远锁住(死了)
        //开启线程
        Thread tdThreadEPC = null;
        Thread tdThreadWrite = null;
        bool nStop = false;

        Thread teThreadEPC = null;//6c读线程
        public int mem, ptr, len;
        public int Read_times;
        public byte[] AccessPassWord = new byte[4];
        public int nBaudRate = 0, Interval, EPC_Word;
        public byte[] IDTemp = new byte[120];
        //================ISO 18000 6C============================================

        //---单标签操作
        Thread TimerEPC = null;//6c读线程

        /// <summary>
        /// 日志文件的宏
        /// </summary>
        private const string LOGDIR = "\\Log\\";

        /// <summary>
        /// 写日志的扩展名
        /// </summary>
        private const string LOGEXT = ".log";
        public static string g_AppLogFileName;

        public Reader.VH_fDechUsb fCallbackFunc = null;


        public FrmDemoWC()
        {
            InitializeComponent();

            //取当前年月日
            DateTime CurrentTime = System.DateTime.Now;
            g_AppLogFileName = System.Environment.CurrentDirectory;
            g_AppLogFileName += LOGDIR;
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(g_AppLogFileName);
            di.Create();

            g_AppLogFileName += CurrentTime.ToString("yyyyMMdd") + LOGEXT;
            Reader.VH_SetLogFile(g_AppLogFileName);

            fCallbackFunc = VH_fDechUsb;
            Reader.VH_InitUsb(fCallbackFunc);

            // new Reader.tagReaderFreq {},new Reader.tagReaderFreq {}
            //{"00---FCC(American))", 63, 400, 902600},							//(0),{"00---FCC(American)", 50, 500, 902750},
            //{"01---ETSI EN 300-220(Europe300-220)", 11, 200, 865500},			//(1),{"01---ETSI EN 300-220(Europe300-220)", -1, -1, -1},
            //{"02---ETSI EN 302-208(Europe302-208)", 4, 600, 865700},			//(2)
            //{"03---HK920-925(Hong Kong)", 10, 500, 920250},						//(3)
            //{"04---TaiWan 922-928(Taiwan)", 12, 500, 922250},					//(4)
            //{"05---Japan 952-954(Japan)", 0, 0, 0},								//(5)
            //{"06---Japan 952-955(Japan)", 14,200, 952200},						//(6)
            //{"07---ETSI EN 302-208(Europe)", 4, 600, 865700},					//(7)
            //{"08---Korea 917-921(Korea)", 6, 600, 917300},						//(8)
            //{"09---Malaysia 919-923(Malaysia)", 8, 500, 919250},				//(9)
            //{"10--China 920-925(China)", 16, 250, 920625},						//(10)
            //{"11--Japan 952-956(Japan)", 4, 1200, 952400},						//(11)
            //{"12--South Africa 915-919(Poncho)", 17, 200, 915600},				//(12)
            //{"13--Brazil 902-907/915-928(Brazil)", 35, 500, 902750},			//(13)
            //{"14--Thailand 920-925(Thailand)", -1, -1, -1},						//(14)
            //{"15--Singapore 920-925(Singapore)", 10, 500, 920250},				//(15)
            //{"16--Australia 920-926(Australia)", 12, 500, 920250},				//(16)
            //{"17--India 865-867(India)", 4, 600, 865100},						//(17)
            //{"18--Uruguay 916-928(Uruguay)", 23, 500, 916250},					//(18)
            //{"19--Vietnam 920-925(Vietnam)", 10, 500, 920250},					//(19)
            //{"20--Israel 915-917", 1, 0, 916250},								//(20)
            //{"21--Philippines 918-920(Philippines)", 4, 500, 918250},			//(21)
            //{"22--Canada 902-928(Canada)", 42, 500, 902750},					//(22)
            //{"23--Indonesia 923-925(Indonesia)", 4, 500, 923250},				//(23)
            //{"24--New Zealand 921.5-928(New Zealand)", 11, 500, 922250},		//(24)
            //};

            stuctFreqCountry[0].chFreq = "00---FCC(American))";                     //国家频率字符串
            stuctFreqCountry[0].iGrade = 63;                                        //级数
            stuctFreqCountry[0].iSkip = 400;                                       //步进
            stuctFreqCountry[0].dwFreq = 902600;                                    //起始频率

            stuctFreqCountry[1].chFreq = "01---ETSI EN 300-220(Europe300-220)";                     //国家频率字符串
            stuctFreqCountry[1].iGrade = 11;                                        //级数
            stuctFreqCountry[1].iSkip = 200;                                       //步进
            stuctFreqCountry[1].dwFreq = 865500;                                    //起始频率

            stuctFreqCountry[2].chFreq = "02---ETSI EN 302-208(Europe302-208)";                     //国家频率字符串
            stuctFreqCountry[2].iGrade = 4;                                        //级数
            stuctFreqCountry[2].iSkip = 600;                                       //步进
            stuctFreqCountry[2].dwFreq = 865700;                                    //起始频率

            stuctFreqCountry[3].chFreq = "03---HK920-925(Hong Kong)";                     //国家频率字符串
            stuctFreqCountry[3].iGrade = 10;                                        //级数
            stuctFreqCountry[3].iSkip = 500;                                       //步进
            stuctFreqCountry[3].dwFreq = 920250;                                    //起始频率

            stuctFreqCountry[4].chFreq = "04---TaiWan 922-928(Taiwan)";                     //国家频率字符串
            stuctFreqCountry[4].iGrade = 12;                                        //级数
            stuctFreqCountry[4].iSkip = 500;                                       //步进
            stuctFreqCountry[4].dwFreq = 922250;                                    //起始频率

            stuctFreqCountry[5].chFreq = "05---Japan 952-954(Japan)";                     //国家频率字符串
            stuctFreqCountry[5].iGrade = 0;                                        //级数
            stuctFreqCountry[5].iSkip = 0;                                       //步进
            stuctFreqCountry[5].dwFreq = 0;                                    //起始频率

            stuctFreqCountry[6].chFreq = "06---Japan 952-955(Japan)";                     //国家频率字符串
            stuctFreqCountry[6].iGrade = 14;                                        //级数
            stuctFreqCountry[6].iSkip = 200;                                       //步进
            stuctFreqCountry[6].dwFreq = 952200;                                    //起始频率

            stuctFreqCountry[7].chFreq = "07---ETSI EN 302-208(Europe)";                     //国家频率字符串
            stuctFreqCountry[7].iGrade = 4;                                        //级数
            stuctFreqCountry[7].iSkip = 600;                                       //步进
            stuctFreqCountry[7].dwFreq = 865700;                                    //起始频率

            stuctFreqCountry[8].chFreq = "08---Korea 917-921(Korea)";                     //国家频率字符串
            stuctFreqCountry[8].iGrade = 6;                                        //级数
            stuctFreqCountry[8].iSkip = 600;                                       //步进
            stuctFreqCountry[8].dwFreq = 917300;                                    //起始频率

            stuctFreqCountry[9].chFreq = "09---Malaysia 919-923(Malaysia)";                     //国家频率字符串
            stuctFreqCountry[9].iGrade = 8;                                        //级数
            stuctFreqCountry[9].iSkip = 500;                                       //步进
            stuctFreqCountry[9].dwFreq = 919250;                                    //起始频率

            stuctFreqCountry[10].chFreq = "10--China 920-925(China)";                     //国家频率字符串
            stuctFreqCountry[10].iGrade = 16;                                        //级数
            stuctFreqCountry[10].iSkip = 250;                                       //步进
            stuctFreqCountry[10].dwFreq = 920625;                                    //起始频率

            stuctFreqCountry[11].chFreq = "11--Japan 952-956(Japan)";                     //国家频率字符串
            stuctFreqCountry[11].iGrade = 4;                                        //级数
            stuctFreqCountry[11].iSkip = 1200;                                       //步进
            stuctFreqCountry[11].dwFreq = 952400;                                    //起始频率

            stuctFreqCountry[12].chFreq = "12--South Africa 915-919(Poncho)";                     //国家频率字符串
            stuctFreqCountry[12].iGrade = 17;                                        //级数
            stuctFreqCountry[12].iSkip = 200;                                       //步进
            stuctFreqCountry[12].dwFreq = 915600;                                    //起始频率

            stuctFreqCountry[13].chFreq = "13--Brazil 902-907/915-928(Brazil)";                     //国家频率字符串
            stuctFreqCountry[13].iGrade = 35;                                        //级数
            stuctFreqCountry[13].iSkip = 500;                                       //步进
            stuctFreqCountry[13].dwFreq = 902750;                                    //起始频率

            stuctFreqCountry[14].chFreq = "14--Thailand 920-925(Thailand)";                     //国家频率字符串
            stuctFreqCountry[14].iGrade = -1;                                        //级数
            stuctFreqCountry[14].iSkip = -1;                                       //步进
            stuctFreqCountry[14].dwFreq = -1;                                    //起始频率

            stuctFreqCountry[15].chFreq = "15--Singapore 920-925(Singapore)";                     //国家频率字符串
            stuctFreqCountry[15].iGrade = 10;                                        //级数
            stuctFreqCountry[15].iSkip = 500;                                       //步进
            stuctFreqCountry[15].dwFreq = 920250;                                    //起始频率

            stuctFreqCountry[16].chFreq = "16--Australia 920-926(Australia)";                     //国家频率字符串
            stuctFreqCountry[16].iGrade = 12;                                        //级数
            stuctFreqCountry[16].iSkip = 500;                                       //步进
            stuctFreqCountry[16].dwFreq = 920250;                                    //起始频率

            stuctFreqCountry[17].chFreq = "17--India 865-867(India)";                     //国家频率字符串
            stuctFreqCountry[17].iGrade = 4;                                        //级数
            stuctFreqCountry[17].iSkip = 600;                                       //步进
            stuctFreqCountry[17].dwFreq = 865100;                                    //起始频率

            stuctFreqCountry[18].chFreq = "18--Uruguay 916-928(Uruguay)";                     //国家频率字符串
            stuctFreqCountry[18].iGrade = 23;                                        //级数
            stuctFreqCountry[18].iSkip = 500;                                       //步进
            stuctFreqCountry[18].dwFreq = 916250;                                    //起始频率

            stuctFreqCountry[19].chFreq = "19--Vietnam 920-925(Vietnam)";                     //国家频率字符串
            stuctFreqCountry[19].iGrade = 10;                                        //级数
            stuctFreqCountry[19].iSkip = 500;                                       //步进
            stuctFreqCountry[19].dwFreq = 920250;                                    //起始频率

            stuctFreqCountry[20].chFreq = "20--Israel 915-917";                     //国家频率字符串
            stuctFreqCountry[20].iGrade = 1;                                        //级数
            stuctFreqCountry[20].iSkip = 0;                                       //步进
            stuctFreqCountry[20].dwFreq = 916250;                                    //起始频率

            stuctFreqCountry[21].chFreq = "21--Philippines 918-920(Philippines)";                     //国家频率字符串
            stuctFreqCountry[21].iGrade = 4;                                        //级数
            stuctFreqCountry[21].iSkip = 500;                                       //步进
            stuctFreqCountry[21].dwFreq = 918250;                                    //起始频率

            stuctFreqCountry[22].chFreq = "22--Canada 902-928(Canada)";                     //国家频率字符串
            stuctFreqCountry[22].iGrade = 42;                                        //级数
            stuctFreqCountry[22].iSkip = 500;                                       //步进
            stuctFreqCountry[22].dwFreq = 902750;                                    //起始频率

            stuctFreqCountry[23].chFreq = "23--Indonesia 923-925(Indonesia)";                     //国家频率字符串
            stuctFreqCountry[23].iGrade = 4;                                        //级数
            stuctFreqCountry[23].iSkip = 500;                                       //步进
            stuctFreqCountry[23].dwFreq = 923250;                                    //起始频率

            stuctFreqCountry[24].chFreq = "24--New Zealand 921.5-928(New Zealand)";                     //国家频率字符串
            stuctFreqCountry[24].iGrade = 11;                                        //级数
            stuctFreqCountry[24].iSkip = 500;                                       //步进
            stuctFreqCountry[24].dwFreq = 922250;                                    //起始频率
        }

        public int VH_fDechUsb(bool bDech)
        {
            if (bDech)
            {
                MessageBox.Show("在线!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show("不在线!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return 0;
        }

        private void FrmDemoWC_Load(object sender, EventArgs e)
        {
            //线程间操作无效: 从不是创建控件“...”的线程访问它(解决方法) 
            Control.CheckForIllegalCrossThreadCalls = false;

            connect_OK = 0;//0--no connect, 1--connected

            //==第一页==========连接页面开始============================
            radioButton1.Checked = false;//RS232
            radioButton2.Checked = true;//USB

            foreach (string portName in SerialPort.GetPortNames())
            {
                comboBox1.Items.Add(portName);
            }

            comboBox1.SelectedIndex = 0;
            comboBox1.Enabled = false;
            comboBox4.SelectedIndex = 0;
            comboBox5.SelectedIndex = 5;
            comboBox9.SelectedIndex = 3;
            comboBox6.Items.Clear();

            tabControl1.SelectedIndex = 0;//进入第1页,connect
        }

        private void FrmDemoWC_FormClosed(object sender, FormClosedEventArgs e)
        {
            //窗体关闭时
            if (0 == connect_OK)
            {
                button2_Click(null, null);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //连接读写器
            int i = 0;
            ComMode = 0;
            if (radioButton1.Checked == true)
            {
                ComMode = 1;//RS232
            }
            if (radioButton2.Checked == true)
            {
                ComMode = 3;//USB
            }

            //COM口
            szPort = comboBox1.Text;

            switch (ComMode)
            {
                case 1:
                    res = Reader.ConnectScanner(ref m_hScanner, szPort, 0);
                    break;
                case 3:
                    res = Reader.VH_ConnectScannerUsb(ref m_hScanner);
                    break;
            }

            if (_OK == res)
            {
                for (i = 0; i < 3; i++)
                {
                    //读版本
                    res = Reader.VH_GetVersion(m_hScanner, ref HardVersion, ref SoftVersion);
                    if (_OK == res)
                    {
                        break;
                    }
                }
                if (res != _OK)
                {
                    MessageBox.Show("Connect Reader Fail!(VH_GetVersion)", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    button2_Click(null, null);
                    return;
                }

                for (i = 0; i < 3; i++)
                {
                    //读基本参数
                    res = Reader.ReadBasicParam(m_hScanner, ref gBasicParam);
                    if (_OK == res)
                    {
                        break;
                    }
                }
                if (res != _OK)
                {
                    MessageBox.Show("Connect Reader Fail!(ReadBasicParam)", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    button2_Click(null, null);
                    return;
                }

                for (i = 0; i < 3; i++)
                {
                    //国家频率
                    res = Reader.GetFrequencyRange(m_hScanner, ref gFre);
                    if (_OK == res)
                    {
                        break;
                    }
                }
                if (res != _OK)
                {
                    MessageBox.Show("Connect Reader Fail!(GetFrequencyRange)", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    button2_Click(null, null);
                    return;
                }

                for (i = 0; i < 3; i++)
                {
                    //读auto参数
                    res = Reader.ReadAutoParam(m_hScanner, ref gAutoParam);
                    if (_OK == res)
                    {
                        break;
                    }
                }
                if (res != _OK)
                {
                    MessageBox.Show("Connect Reader Fail!(ReadAutoParam)", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    button2_Click(null, null);
                    return;
                }

                for (i = 0; i < 3; i++)
                {
                    //读ID
                    res = Reader.GetReaderID(m_hScanner, gReaderID);
                    if (_OK == res)
                    {
                        break;
                    }
                }
                if (res != _OK)
                {
                    MessageBox.Show("Connect Reader Fail!(GetReaderID)", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    button2_Click(null, null);
                    return;
                }

                for (i = 0; i < 3; i++)
                {
                    byte[] pPParam = new byte[26];
                    //读仿真参数
                    res = Reader.ReadSimParam(m_hScanner, ref Param);
                    if (_OK == res)
                    {
                        break;
                    }
                }
                if (res != _OK)
                {
                    MessageBox.Show("Connect Reader Fail!(ReadSimParam)", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    button2_Click(null, null);
                    return;
                }

                //弹出提示成功
                connect_OK = 1;//0--no connect, 1--connected

                button1.Enabled = false;//ConnectReader
                button2.Enabled = true;//DisconnectReader

                tabControl1.SelectedIndex = 2;//进入第3页,ISO18000-6C
                ParamSettoUI();
                MessageBox.Show("Connect Reader Success!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                button1.Enabled = true;//ConnectReader
                button2.Enabled = false;//DisconnectReader
                MessageBox.Show("Connect Reader Fail!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //断开连接读写器
            switch (ComMode)
            {
                case 1:
                    res = Reader.DisconnectScanner(m_hScanner);
                    break;
                case 3:
                    res = Reader.VH_DisconnectScannerUsb(m_hScanner);
                    break;
            }

            button1.Enabled = true;//ConnectReader
            button2.Enabled = false;//DisconnectReader
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            //RS232
            comboBox1.Enabled = true;
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            //USB
            comboBox1.Enabled = false;
        }

        //sel为国家频率下标,imin为最小频率下标,imax为最大频率下标, -1为最小为最小，最大为最大
        public void OnCbnSetFre(int sel, int imin, int imax)
        {
            int i = 0;
            int k = 0;
            int iCount = 0;
            long iValue = 0;
            string strFreqTmp;
            Reader.tagReaderFreq tmpFreq;

            //频率计算公式
            //级数 = 50;
            //步进 = 500KHz;
            //起始频率 = 902750;
            //902750 + 级数*步进;
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            switch (sel)
            {
                case 0://{"0---FCC（美国）", 50, 500, 902750},								//(0)
                case 1://{"1---ETSI EN 300-220（欧洲300-220）", -1, -1, -1},				//(1)
                case 2://{"2---ETSI EN 302-208（欧洲302-208）", 4, 600, 865700},			//(2)
                case 3://"3---HK920-925香港", 10, 500, 920250},							//(3)
                case 4://{"4---TaiWan 922-928台湾", 12, 500, 922250},						//(4)
                case 5://{"5---Japan 952-954日本", 0, 0, 0},								//(5)
                case 6://{"6---Japan 952-955日本", 14,200, 952200},						//(6)
                case 7://{"7---ETSI EN 302-208欧洲", 4, 600, 865700},						//(7)
                case 8://{"8---Korea 917-921韩国", 6, 600, 917300},						//(8)
                case 9://{"9---Malaysia 919-923马来西亚", 8, 500, 919250},					//(9)
                case 10://{"10--China 920-925中国", 16, 250, 920625},						//(10)
                case 11://{"11--Japan 952-956日本", 4, 1200, 952400},						//(11)
                case 12://{"12--South Africa 915-919南美", 17, 200, 915600},				//(12)
                case 13://{"13--Brazil 902-907/915-928巴西", 35, 500, 902750},				//(13)
                case 14://{"14--Thailand 920-925泰国", -1, -1, -1},						//(14)
                case 15://{"15--Singapore 920-925新加坡", 10, 500, 920250},				//(15)
                case 16://{"16--Australia 920-926澳洲", 12, 500, 920250},					//(16)
                case 17://{"17--India 865-867印度", 4, 600, 865100},						//(17)
                case 18://{"18--Uruguay 916-928乌拉圭", 23, 500, 916250},					//(18)
                case 19://{"19--Vietnam 920-925越南", 10, 500, 920250},					//(19)
                case 20://{"20--Israel 915-917", 1, 0, 916250},							//(20)
                case 21://{"21--Philippines 918-920菲律宾", 4, 500, 918250},				//(21)
                case 22://{"22--Canada 902-928加拿大", 42, 500, 902750},					//(22)
                case 23://{"23--Indonesia 923-925印度尼西亚", 4, 500, 923250},				//(23)
                case 24://{"24--New Zealand 921.5-928新西兰", 11, 500, 922250},			//(24)
                    tmpFreq.iGrade = stuctFreqCountry[sel].iGrade;
                    tmpFreq.iSkip = stuctFreqCountry[sel].iSkip;
                    tmpFreq.dwFreq = stuctFreqCountry[sel].dwFreq;
                    k = 0;
                    iCount = tmpFreq.iGrade;

                    if (22 == sel)
                    {
                        int[] iArray = new int[3] { 902750, 903250, 905750 };
                        //加拿大的特述
                        for (i = 0; i < 3; i++)
                        {
                            strFreqTmp = string.Format("{0,0:d02}{1,0:S}{2,0:d}", i + 1, "--", iArray[i]);

                            comboBox2.Items.Add(strFreqTmp);
                            comboBox3.Items.Add(strFreqTmp);
                        }
                        tmpFreq.dwFreq = iArray[i - 1];
                        iCount = tmpFreq.iGrade - 3;
                        k = 3;
                    }

                    for (i = k; i < iCount + k; i++)
                    {
                        iValue = tmpFreq.dwFreq + i * tmpFreq.iSkip;
                        strFreqTmp = string.Format("{0,0:d02}{1,0:S}{2,0:D}", i + 1, "--", iValue);
                        comboBox2.Items.Add(strFreqTmp);
                        comboBox3.Items.Add(strFreqTmp);
                    }

                    iCount = tmpFreq.iGrade;
                    if (i > 0)
                    {
                        if (imin == -1 && imax == -1)
                        {
                            comboBox2.SelectedIndex = 0;
                            comboBox3.SelectedIndex = (iCount - 1);
                        }
                        else
                        {
                            comboBox2.SelectedIndex = imin;
                            comboBox3.SelectedIndex = imax;
                        }
                    }

                    break;
            }
        }

        #region 参数页面
        //==第二页==========参数页面开始============================
        private void ParamSettoUI()
        {
            string strTemp;
            strTemp = string.Format("{0,0:x04}", HardVersion);
            //硬件版本和软件版本
            textBox1.Text = String.Format("{0,0:D02}{1,0:D02}", (byte)(HardVersion >> 8), (byte)HardVersion);
            textBox2.Text = String.Format("{0,0:D02}{1,0:D02}", (byte)(SoftVersion >> 8), (byte)SoftVersion);

            //固件的ID号
            //读写器ID
            textBox3.Text = System.Text.Encoding.UTF8.GetString(gReaderID);

            //最小频率
            //最大频率
            byte imin = (byte)(gBasicParam.Min_Frequence - 1);
            byte imax = (byte)(gBasicParam.Max_Frequence - 1);
            OnCbnSetFre(gFre, imin, imax);//因为通过国家来，每个国家的频率有些不一样的

            //RF power
            strTemp = string.Format("{0,0:d}", gBasicParam.Power);
            textBox4.Text = strTemp;

            //哪种标签//哪种标签
            //0-6B,1-6C
            //0-6B,1-6C
            if ((gBasicParam.TagType & 0x01) == 1)
            {
                radioButton3.Checked = true;
            }

            if ((gBasicParam.TagType & 0x04) == 4)
            {
                radioButton4.Checked = true;
            }

            //定时间隔
            //0-10ms，1-20ms，2-30ms，3-50ms，4-100ms。缺省值为2。每隔设定时间主动读取一次标签。
            comboBox4.SelectedIndex = gAutoParam.Interval;

            //////////////////////////////////////////////////////////////////////////
            //16进制输出，还是10进制输出
            //0-16进,1-10进
            if (0 == Param.DataFormat)
            {
                radioButton5.Checked = true;
                radioButton6.Checked = false;
            }
            else if (1 == Param.DataFormat)
            {
                radioButton5.Checked = false;
                radioButton6.Checked = true;
            }

            //数据区
            //(1)EPC区--0, USER区--1，TID区--2,add by mqs20131216改成了取自动模式下的参数
            //if ( Param.DataBank == 0 )
            if (gAutoParam.IDPosition == 0)
            {
                radioButton7.Checked = true;
                radioButton8.Checked = false;
                radioButton9.Checked = false;
            }
            else if (gAutoParam.IDPosition == 1)
            {
                radioButton7.Checked = false;
                radioButton8.Checked = true;
                radioButton9.Checked = false;
            }
            else if (gAutoParam.IDPosition == 2)
            {
                radioButton7.Checked = false;
                radioButton8.Checked = false;
                radioButton9.Checked = true;
            }

            //是否前缀，1--带,0--不带		
            checkBox1.Checked = Convert.ToBoolean(Param.IsPerfix);

            //前缀值,不够0x00填充		
            //strTemp = string.Format("{0,0:d}", Param.PerfixCode);
            textBox5.Text = System.Text.Encoding.UTF8.GetString(Param.PerfixCode);

            //是否回车换行, 1--带,0--不带	
            checkBox2.Checked = Convert.ToBoolean(Param.IsEnter);

            //数据起始地址
            strTemp = string.Format("{0,0:d}", Param.StartAddress);
            textBox6.Text = strTemp;

            //数据长度
            strTemp = string.Format("{0,0:d}", Param.DataLen);
            textBox7.Text = strTemp;

            //标准输出间隔(0-255)(0--输出ID之间无间隔)
            strTemp = string.Format("{0,0:d}", Param.OutputInterval);
            textBox8.Text = strTemp;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int i = 0;

            for (i = 0; i < 3; i++)
            {
                //读版本
                res = Reader.VH_GetVersion(m_hScanner, ref HardVersion, ref SoftVersion);
                if (_OK == res)
                {
                    break;
                }
            }
            if (res != _OK)
            {
                MessageBox.Show("Reader Fail!(VH_GetVersion)", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //获取参数
            for (i = 0; i < 3; i++)
            {
                //读基本参数
                res = Reader.ReadBasicParam(m_hScanner, ref gBasicParam);
                if (_OK == res)
                {
                    break;
                }
            }
            if (res != _OK)
            {
                MessageBox.Show("Reader Fail!(ReadBasicParam)", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            for (i = 0; i < 3; i++)
            {
                //国家频率
                res = Reader.GetFrequencyRange(m_hScanner, ref gFre);
                if (_OK == res)
                {
                    break;
                }
            }
            if (res != _OK)
            {
                MessageBox.Show("Reader Fail!(GetFrequencyRange)", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            for (i = 0; i < 3; i++)
            {
                //读auto参数
                res = Reader.ReadAutoParam(m_hScanner, ref gAutoParam);
                if (_OK == res)
                {
                    break;
                }
            }
            if (res != _OK)
            {
                MessageBox.Show("Reader Fail!(ReadAutoParam)", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            for (i = 0; i < 3; i++)
            {
                byte[] pPParam = new byte[26];
                //读仿真参数
                res = Reader.ReadSimParam(m_hScanner, ref Param);
                if (_OK == res)
                {
                    break;
                }
            }
            if (res != _OK)
            {
                MessageBox.Show("Reader Fail!(ReadSimParam)", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ParamSettoUI();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //更新参数
            int i = 0;
            string strText = string.Empty;
            int iValue = 0;

            //最小频率
            //最大频率
            int imin = comboBox2.SelectedIndex;
            int imax = comboBox3.SelectedIndex;
            if (imin < 0)
            {
                imin = 0;
            }
            if (imax < 0)
            {
                imax = 0;
            }
            //因为通过国家来，每个国家的频率有些不一样的
            gBasicParam.Min_Frequence = (byte)(imin + 1);
            gBasicParam.Max_Frequence = (byte)(imax + 1);

            //RF power
            strText = textBox4.Text;
            iValue = 0;
            int.TryParse(strText, out iValue);//字符串转数字
            if ((iValue < 0) || (iValue > 63))
            {
                iValue = 63;
            }
            gBasicParam.Power = (byte)iValue;

            //哪种标签,//0-6B,1-6C
            gBasicParam.TagType = 0x00;
            //6C
            if (radioButton3.Checked == true)
            {
                gBasicParam.TagType = 0x01;
            }
            if (radioButton4.Checked == true)
            {
                gBasicParam.TagType = 0x04;
            }

            for (i = 0; i < 3; i++)
            {
                //写基本参数
                res = Reader.WriteBasicParam(m_hScanner, ref gBasicParam);
                if (_OK == res)
                {
                    break;
                }
            }

            if (_OK != res)
            {
                MessageBox.Show("Set fail0!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //定时间隔
            //0-10ms，1-20ms，2-30ms，3-50ms，4-100ms。缺省值为2。每隔设定时间主动读取一次标签。
            gAutoParam.Interval = (byte)comboBox4.SelectedIndex;

            //数据区
            //(1)EPC区--0, USER区--1，TID区--2,add by mqs20131216改成了取自动模式下的参数
            if (radioButton7.Checked == true)
            {
                gAutoParam.IDPosition = 0;
            }
            if (radioButton8.Checked == true)
            {
                gAutoParam.IDPosition = 1;
            }
            if (radioButton9.Checked == true)
            {
                gAutoParam.IDPosition = 2;
            }

            for (i = 0; i < 3; i++)
            {
                //写auto参数
                res = Reader.WriteAutoParam(m_hScanner, ref gAutoParam);
                if (_OK == res)
                {
                    break;
                }
            }

            if (_OK != res)
            {
                MessageBox.Show("Set fail1!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //////////////////////////////////////////////////////////////////////////
            //16进制输出，还是10进制输出
            //0-16进,1-10进
            if (radioButton5.Checked == true)
            {
                Param.DataFormat = 0;
            }
            if (radioButton6.Checked == true)
            {
                Param.DataFormat = 1;
            }

            //是否前缀，1--带,0--不带		
            if (checkBox1.Checked == true)
            {
                Param.IsPerfix = 1;
            }
            else
            {
                Param.IsPerfix = 0;
            }

            //前缀值,不够0x00填充		
            strText = textBox5.Text;
            byte[] ssb = System.Text.Encoding.UTF8.GetBytes(strText);
            for (i = 0; i < ssb.Length; i++)
            {
                Param.PerfixCode[i] = ssb[i];
            }
            Param.PerfixCode[i] = 0;

            //是否回车换行, 1--带,0--不带
            if (checkBox2.Checked == true)
            {
                Param.IsEnter = 1;
            }
            else
            {
                Param.IsEnter = 0;
            }

            //数据起始地址
            strText = textBox6.Text;
            iValue = 0;
            int.TryParse(strText, out iValue);//字符串转数字
            Param.StartAddress = (byte)iValue;

            //数据长度
            strText = textBox7.Text;
            iValue = 0;
            int.TryParse(strText, out iValue);//字符串转数字
            Param.DataLen = (byte)iValue;

            //标准输出间隔(0-255)(0--输出ID之间无间隔)
            strText = textBox8.Text;
            iValue = 0;
            int.TryParse(strText, out iValue);//字符串转数字
            Param.OutputInterval = (byte)iValue;

            for (i = 0; i < 3; i++)
            {
                //写仿真参数
                res = Reader.WriteSimParam(m_hScanner, ref Param);
                if (_OK == res)
                {
                    break;
                }
            }

            if (_OK != res)
            {
                MessageBox.Show("Set fail2!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }

            MessageBox.Show("Set paramter successful!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        //==================参数页面结束============================
        #endregion

        #region ISO18000-6C----页面

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            //Use password to write
            checkBox6.Checked = false;
            textBox20.Enabled = true;
            textBox20.Focus();
        }

        private void checkBox4_Click(object sender, EventArgs e)
        {
            //Use password to write
            checkBox6.Checked = false;
            textBox20.Enabled = true;
            textBox20.Focus();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            //Never be written
            checkBox4.Checked = false;
            textBox20.Enabled = false;
        }

        private void checkBox6_Click(object sender, EventArgs e)
        {
            //Never be written
            checkBox4.Checked = false;
            textBox20.Enabled = false;
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            string m_DisStr = "";
            m_Word = comboBox5.SelectedIndex;
            button5.Enabled = true;
            switch (m_Word)
            {
                case 1:
                    m_DisStr = ("0000");
                    button5.Enabled = false;
                    break;
                case 2:
                    m_DisStr = ("0000-0000");
                    break;
                case 3:
                    m_DisStr = ("0000-0000-0000");
                    break;
                case 4:
                    m_DisStr = ("0000-0000-0000-0000");
                    break;
                case 5:
                    m_DisStr = ("0000-0000-0000-0000-0000");
                    break;
                case 6:
                    m_DisStr = ("0000-0000-0000-0000-0000-0000");
                    break;
                case 7:
                    m_DisStr = ("0000-0000-0000-0000-0000-0000-0000");
                    break;
                case 8:
                    m_DisStr = ("0000-0000-0000-0000-0000-0000-0000-0000");
                    break;
                default:
                    m_DisStr = "";
                    button5.Enabled = false;
                    break;
            }

            textBox9.Text = m_DisStr;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ////按规则生成数据
            //string epcData = "";
            //string strText = "";
            //string strTemp = "";
            //int iValue = 0;
            //int m_Count = 0;
            //int m_Sum = 0;
            //int i = 0;

            ////步长
            //strText = textBox13.Text;
            //iValue = 0;
            //int.TryParse(strText, out iValue);//字符串转数字
            //if (iValue <= 0)
            //{
            //    MessageBox.Show("Incremental step never ≤ 0", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return;
            //}
            //m_Count = iValue;

            ////总数
            //strText = textBox14.Text;
            //iValue = 0;
            //int.TryParse(strText, out iValue);//字符串转数字
            //if (iValue <= 0)
            //{
            //    MessageBox.Show("Data count never ≤ 0", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return;
            //}
            //m_Sum = iValue;

            //int j = m_WriteMode + 1;

            ////前缀和后缀的选择
            //bool m_CBefor = checkBox3.Checked;
            //bool m_CEnd = checkBox5.Checked;

            //if ((m_CBefor) && (!m_CEnd))//前缀打勾，后缀不打勾
            //{
            //    string m_Befro = textBox10.Text;//前缀数据
            //    int nBLen = m_Befro.Length;
            //    if ((nBLen < 4) || ((nBLen % 4) != 0) || (nBLen >= m_Word * 4))
            //    {
            //        MessageBox.Show("前缀数据错误", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //        return;
            //    }

            //    listView1.Items.Clear();

            //    strTemp = textBox11.Text;//起始序号

            //    int nB = 0;

            //    for (i = 0; i < m_Sum; i++)//总数
            //    {
            //        switch ((((m_Word * 4) - nBLen) / 4))
            //        {
            //            case 1:
            //                epcData = string.Format("{0,0:s}{1,4:s}", m_Befro, strTemp);
            //                break;
            //            case 2:
            //                epcData = string.Format("{0,0:s}{1,8:s}", m_Befro, strTemp);
            //                break;
            //            case 3:
            //                epcData = string.Format("{0,0:s}{1,12:s}", m_Befro, strTemp);
            //                break;
            //            case 4:
            //                epcData = string.Format("{0,0:s}{1,16:s}", m_Befro, strTemp);
            //                break;
            //            case 5:
            //                epcData = string.Format("{0,0:s}{1,20:s}", m_Befro, strTemp);
            //                break;
            //            case 6:
            //                epcData = string.Format("{0,0:s}{1,24:s}", m_Befro, strTemp);
            //                break;
            //            case 7:
            //                epcData = string.Format("{0,0:s}{1,28:s}", m_Befro, strTemp);
            //                break;
            //            default:
            //                epcData = string.Format("{0,0:s}{1,32:s}", m_Befro, strTemp);
            //                break;
            //        }
            //        epcData = epcData.Replace(" ", "0");
            //        nB = nB + m_Count;
            //        string chixA = "";
            //        string chixB = "";
            //        //读取版本
            //        StringBuilder chixC = new StringBuilder(255);
            //        chixA = strTemp;
            //        chixB = string.Format("{0,0:x}", m_Count);
            //        Reader.VH_big_num_add(chixC, chixA, chixB);
            //        strTemp = "";

            //        strTemp += chixC;

            //        epcData = epcData.ToUpper();

            //        int nS = listView1.Items.Count;

            //        chixA = string.Format("{0,0:d}", nS + 1);

            //        ListViewItem item = new ListViewItem();
            //        item = listView1.Items.Add((nS + 0).ToString(), chixA);
            //        item.SubItems.Add(epcData);
            //        item.SubItems.Add("");
            //        item.SubItems.Add("");
            //    }
            //}

            //if ((m_CBefor) && (m_CEnd))//前缀打勾，后缀打勾
            //{
            //    string m_Befro;
            //    string m_End;
            //    m_Befro = textBox10.Text;//前缀数据
            //    int nBLen = m_Befro.Length;
            //    if ((nBLen < 4) || ((nBLen % 4) != 0) || (nBLen >= m_Word * 4))
            //    {
            //        MessageBox.Show("前缀数据错误", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //        return;
            //    }

            //    m_End = textBox12.Text;//前缀数据
            //    int nELen = m_End.Length;
            //    if ((nELen < 4) || ((nELen % 4) != 0) || (nELen >= m_Word * 4))
            //    {
            //        MessageBox.Show("后缀数据错误", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //        return;
            //    }

            //    if ((nBLen + nELen) >= m_Word * 4)
            //    {
            //        MessageBox.Show("前缀和后缀数据长度错误", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //        return;
            //    }

            //    listView1.Items.Clear();

            //    strTemp = textBox11.Text;//起始序号

            //    int nB = 0;
            //    for (i = 0; i < m_Sum; i++)//总数
            //    {
            //        switch ((((m_Word * 4) - nBLen - nELen) / 4))
            //        {
            //            case 1:
            //                epcData = string.Format("{0,0:s}{1,4:s}{2,0:s}", m_Befro, strTemp, m_End);
            //                break;
            //            case 2:
            //                epcData = string.Format("{0,0:s}{1,8:s}{2,0:s}", m_Befro, strTemp, m_End);
            //                break;
            //            case 3:
            //                epcData = string.Format("{0,0:s}{1,12:s}{2,0:s}", m_Befro, strTemp, m_End);
            //                break;
            //            case 4:
            //                epcData = string.Format("{0,0:s}{1,16:s}{2,0:s}", m_Befro, strTemp, m_End);
            //                break;
            //            case 5:
            //                epcData = string.Format("{0,0:s}{1,20:s}{2,0:s}", m_Befro, strTemp, m_End);
            //                break;
            //            case 6:
            //                epcData = string.Format("{0,0:s}{1,24:s}{2,0:s}", m_Befro, strTemp, m_End);
            //                break;
            //            case 7:
            //                epcData = string.Format("{0,0:s}{1,28:s}{2,0:s}", m_Befro, strTemp, m_End);
            //                break;
            //            default:
            //                epcData = string.Format("{0,0:s}{1,32:s}{2,0:s}", m_Befro, strTemp, m_End);
            //                break;
            //        }
            //        epcData = epcData.Replace(" ", "0");
            //        nB = nB + m_Count;
            //        string chixA = "";
            //        string chixB = "";
            //        //读取版本
            //        StringBuilder chixC = new StringBuilder(255);
            //        chixA = strTemp;
            //        chixB = string.Format("{0,0:x}", m_Count);
            //        Reader.VH_big_num_add(chixC, chixA, chixB);
            //        strTemp = "";
            //        strTemp += chixC;

            //        epcData = epcData.ToUpper();

            //        int nS = listView1.Items.Count;

            //        chixA = string.Format("{0,0:d}", nS + 1);

            //        ListViewItem item = new ListViewItem();
            //        item = listView1.Items.Add((nS + 0).ToString(), chixA);
            //        item.SubItems.Add(epcData);
            //        item.SubItems.Add("");
            //        item.SubItems.Add("");
            //    }
            //}

            //if ((!m_CBefor) && (m_CEnd))//前缀不打勾，后缀打勾
            //{
            //    string m_End;
            //    m_End = textBox12.Text;//后缀数据
            //    int nELen = m_End.Length;
            //    if ((nELen < 4) || ((nELen % 4) != 0) || (nELen >= m_Word * 4))
            //    {
            //        MessageBox.Show("后缀数据错误", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //        return;
            //    }

            //    listView1.Items.Clear();

            //    strTemp = textBox11.Text;//起始序号

            //    int nB = 0;
            //    for (i = 0; i < m_Sum; i++)//总数
            //    {
            //        switch ((((m_Word * 4) - nELen) / 4))
            //        {
            //            case 1:
            //                epcData = string.Format("{0,4:s}{1,0:s}", strTemp, m_End);
            //                break;
            //            case 2:
            //                epcData = string.Format("{0,8:s}{1,0:s}", strTemp, m_End);
            //                break;
            //            case 3:
            //                epcData = string.Format("{0,12:s}{1,0:s}", strTemp, m_End);
            //                break;
            //            case 4:
            //                epcData = string.Format("{0,16:s}{1,0:s}", strTemp, m_End);
            //                break;
            //            case 5:
            //                epcData = string.Format("{0,20:s}{1,0:s}", strTemp, m_End);
            //                break;
            //            case 6:
            //                epcData = string.Format("{0,24:s}{1,0:s}", strTemp, m_End);
            //                break;
            //            case 7:
            //                epcData = string.Format("{0,28:s}{1,0:s}", strTemp, m_End);
            //                break;
            //            default:
            //                epcData = string.Format("{0,32:s}{1,0:s}", strTemp, m_End);
            //                break;
            //        }
            //        epcData = epcData.Replace(" ", "0");
            //        nB = nB + m_Count;
            //        string chixA = "";
            //        string chixB = "";
            //        //读取版本
            //        StringBuilder chixC = new StringBuilder(255);
            //        chixA = strTemp;
            //        chixB = string.Format("{0,0:x}", m_Count);
            //        Reader.VH_big_num_add(chixC, chixA, chixB);
            //        strTemp = "";
            //        strTemp += chixC;

            //        epcData = epcData.ToUpper();

            //        int nS = listView1.Items.Count;

            //        chixA = string.Format("{0,0:d}", nS + 1);

            //        ListViewItem item = new ListViewItem();
            //        item = listView1.Items.Add((nS + 0).ToString(), chixA);
            //        item.SubItems.Add(epcData);
            //        item.SubItems.Add("");
            //        item.SubItems.Add("");
            //    }
            //}

            //if ((!m_CBefor) && (!m_CEnd))//前缀不打勾，后缀不打勾
            //{
            //    listView1.Items.Clear();

            //    strTemp = textBox11.Text;//起始序号

            //    int nB = 0;
            //    for (i = 0; i < m_Sum; i++)//总数
            //    {
            //        switch (m_Word)
            //        {
            //            case 1:
            //                epcData = string.Format("{0,4:s}", strTemp);
            //                break;
            //            case 2:
            //                epcData = string.Format("{0,8:s}", strTemp);
            //                break;
            //            case 3:
            //                epcData = string.Format("{0,12:s}", strTemp);
            //                break;
            //            case 4:
            //                epcData = string.Format("{0,16:s}", strTemp);
            //                break;
            //            case 5:
            //                epcData = string.Format("{0,20:s}", strTemp);
            //                break;
            //            case 6:
            //                epcData = string.Format("{0,24:s}", strTemp);
            //                break;
            //            case 7:
            //                epcData = string.Format("{0,28:s}", strTemp);
            //                break;
            //            default:
            //                epcData = string.Format("{0,32:s}", strTemp);
            //                break;
            //        }
            //        epcData = epcData.Replace(" ", "0");
            //        nB = nB + m_Count;
            //        string chixA = "";
            //        string chixB = "";
            //        //读取版本
            //        StringBuilder chixC = new StringBuilder(255);
            //        chixA = strTemp;
            //        chixB = string.Format("{0,0:x}", m_Count);
            //        Reader.VH_big_num_add(chixC, chixA, chixB);
            //        strTemp = "";
            //        strTemp += chixC;

            //        epcData = epcData.ToUpper();

            //        int nS = listView1.Items.Count;

            //        chixA = string.Format("{0,0:d}", nS + 1);

            //        ListViewItem item = new ListViewItem();
            //        item = listView1.Items.Add((nS + 0).ToString(), chixA);
            //        item.SubItems.Add(epcData);
            //        item.SubItems.Add("");
            //        item.SubItems.Add("");
            //    }
            //}

            GetDataInfo();

            //textBox15.Text = m_Sum.ToString();   //总数
            //textBox16.Text = "0";                //完成数
            //textBox17.Text = m_Sum.ToString();   //剩余数
            //textBox19.Text = "0";   //成功数
            //textBox18.Text = "0";   //失败数
        }


        public void GetDataInfo()
        {
            //按规则生成数据
            string epcData = "";
            string strText = "";
            string strTemp = "";
            int iValue = 0;
            int m_Count = 0;
            int m_Sum = 0;
            int i = 0;
            double num = Convert.ToDouble(comboBox5.Text);
            //步长
            strText = textBox13.Text;
            iValue = 0;
            int.TryParse(strText, out iValue);//字符串转数字
            if (iValue < 0)
            {
                MessageBox.Show("累加步长小于0", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            m_Count = iValue;
            //总数
            strText = textBox14.Text;
            iValue = 0;
            int.TryParse(strText, out iValue);//字符串转数字
            if (iValue <= 0)
            {
                MessageBox.Show("生成数量永远不小于等于0", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            m_Sum = iValue;
            //前缀和后缀的选择
            bool m_CBefor = checkBox3.Checked;
            bool m_CEnd = checkBox5.Checked;

            if ((m_CBefor) && (!m_CEnd))//前缀打勾，后缀不打勾
            {
                string m_Befro;
                m_Befro = textBox10.Text;//前缀数据
                int nBLen = m_Befro.Length;
                if ((nBLen < 4) || ((nBLen % 4) != 0) || (nBLen >= m_Word * 4))
                {
                    MessageBox.Show("前缀数据错误", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                listView1.Items.Clear();

                strTemp = textBox11.Text;//起始序号

                int nB = 0;

                for (i = 0; i < m_Sum; i++)//总数
                {
                    switch ((((m_Word * 4) - nBLen) / 4))
                    {
                        case 1:
                            epcData = string.Format("{0,0:s}{1,4:s}", m_Befro, strTemp);
                            break;
                        case 2:
                            epcData = string.Format("{0,0:s}{1,8:s}", m_Befro, strTemp);
                            break;
                        case 3:
                            epcData = string.Format("{0,0:s}{1,12:s}", m_Befro, strTemp);
                            break;
                        case 4:
                            epcData = string.Format("{0,0:s}{1,16:s}", m_Befro, strTemp);
                            break;
                        case 5:
                            epcData = string.Format("{0,0:s}{1,20:s}", m_Befro, strTemp);
                            break;
                        case 6:
                            epcData = string.Format("{0,0:s}{1,24:s}", m_Befro, strTemp);
                            break;
                        case 7:
                            epcData = string.Format("{0,0:s}{1,28:s}", m_Befro, strTemp);
                            break;
                        default:
                            epcData = string.Format("{0,0:s}{1,32:s}", m_Befro, strTemp);
                            break;
                    }

                    epcData = epcData.Replace(" ", "0");

                    double datalenth = epcData.Length / 4.0;

                    if (num != datalenth)
                    {
                        MessageBox.Show("数据输入不规范", "提示");
                        return;
                    }

                    nB = nB + m_Count;

                    string chixA = "";

                    strTemp = (Convert.ToInt32(textBox11.Text, 16) + Convert.ToInt32(nB)).ToString("X");

                    epcData = epcData.ToUpper();

                    int nS = listView1.Items.Count;

                    chixA = string.Format("{0,0:d}", nS + 1);

                    ListViewItem item = new ListViewItem();

                    item = listView1.Items.Add((nS + 0).ToString(), chixA);
                    if (radioButton10.Checked)
                    {
                        item.SubItems.Add(epcData);
                        item.SubItems.Add("");
                        item.SubItems.Add("");
                    }
                    else if (radioButton12.Checked)
                    {
                        item.SubItems.Add("");
                        item.SubItems.Add(epcData);
                        item.SubItems.Add("");
                    }
                    else if (radioButton11.Checked)
                    {
                        item.SubItems.Add("");
                        item.SubItems.Add("");
                        item.SubItems.Add(epcData);
                    }
                }
            }

            if ((m_CBefor) && (m_CEnd))//前缀打勾，后缀打勾
            {
                string m_Befro;
                string m_End;
                m_Befro = textBox10.Text;//前缀数据
                int nBLen = m_Befro.Length;
                if ((nBLen < 4) || ((nBLen % 4) != 0) || (nBLen >= m_Word * 4))
                {
                    MessageBox.Show("前缀数据错误", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                m_End = textBox12.Text;//后缀数据
                int nELen = m_End.Length;
                if ((nELen < 4) || ((nELen % 4) != 0) || (nELen >= m_Word * 4))
                {
                    MessageBox.Show("后缀数据错误", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if ((nBLen + nELen) >= m_Word * 4)
                {
                    MessageBox.Show("前缀和后缀数据长度错误", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                listView1.Items.Clear();

                strTemp = textBox11.Text;//起始序号

                int nB = 0;
                for (i = 0; i < m_Sum; i++)//总数
                {
                    switch ((((m_Word * 4) - nBLen - nELen) / 4))
                    {
                        case 1:
                            epcData = string.Format("{0,0:s}{1,4:s}{2,0:s}", m_Befro, strTemp, m_End);
                            break;
                        case 2:
                            epcData = string.Format("{0,0:s}{1,8:s}{2,0:s}", m_Befro, strTemp, m_End);
                            break;
                        case 3:
                            epcData = string.Format("{0,0:s}{1,12:s}{2,0:s}", m_Befro, strTemp, m_End);
                            break;
                        case 4:
                            epcData = string.Format("{0,0:s}{1,16:s}{2,0:s}", m_Befro, strTemp, m_End);
                            break;
                        case 5:
                            epcData = string.Format("{0,0:s}{1,20:s}{2,0:s}", m_Befro, strTemp, m_End);
                            break;
                        case 6:
                            epcData = string.Format("{0,0:s}{1,24:s}{2,0:s}", m_Befro, strTemp, m_End);
                            break;
                        case 7:
                            epcData = string.Format("{0,0:s}{1,28:s}{2,0:s}", m_Befro, strTemp, m_End);
                            break;
                        default:
                            epcData = string.Format("{0,0:s}{1,32:s}{2,0:s}", m_Befro, strTemp, m_End);
                            break;
                    }

                    epcData = epcData.Replace(" ", "0");

                    double datalenth = epcData.Length / 4.0;

                    if (num != datalenth)
                    {
                        MessageBox.Show("数据输入不规范", "提示");
                        return;
                    }
                    nB = nB + m_Count;

                    string chixA = "";

                    strTemp = "";

                    strTemp = (Convert.ToInt64(textBox11.Text, 16) + Convert.ToInt64(nB)).ToString("X");

                    epcData = epcData.ToUpper();

                    int nS = listView1.Items.Count;

                    chixA = string.Format("{0,0:d}", nS + 1);

                    ListViewItem item = new ListViewItem();

                    item = listView1.Items.Add((nS + 0).ToString(), chixA);

                    if (radioButton10.Checked)
                    {
                        item.SubItems.Add(epcData);
                        item.SubItems.Add("");
                        item.SubItems.Add("");
                    }
                    else if (radioButton12.Checked)
                    {
                        item.SubItems.Add("");
                        item.SubItems.Add(epcData);
                        item.SubItems.Add("");
                    }
                    else if (radioButton11.Checked)
                    {
                        item.SubItems.Add("");
                        item.SubItems.Add("");
                        item.SubItems.Add(epcData);
                    }
                }
            }

            if ((!m_CBefor) && (m_CEnd))//前缀不打勾，后缀打勾
            {
                string m_End;
                m_End = textBox12.Text;//后缀数据
                int nELen = m_End.Length;
                if ((nELen < 4) || ((nELen % 4) != 0) || (nELen >= m_Word * 4))
                {
                    MessageBox.Show("后缀数据错误", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                listView1.Items.Clear();

                strTemp = textBox11.Text;//起始序号

                int nB = 0;
                for (i = 0; i < m_Sum; i++)//总数
                {
                    switch ((((m_Word * 4) - nELen) / 4))
                    {
                        case 1:
                            epcData = string.Format("{0,4:s}{1,0:s}", strTemp, m_End);
                            break;
                        case 2:
                            epcData = string.Format("{0,8:s}{1,0:s}", strTemp, m_End);
                            break;
                        case 3:
                            epcData = string.Format("{0,12:s}{1,0:s}", strTemp, m_End);
                            break;
                        case 4:
                            epcData = string.Format("{0,16:s}{1,0:s}", strTemp, m_End);
                            break;
                        case 5:
                            epcData = string.Format("{0,20:s}{1,0:s}", strTemp, m_End);
                            break;
                        case 6:
                            epcData = string.Format("{0,24:s}{1,0:s}", strTemp, m_End);
                            break;
                        case 7:
                            epcData = string.Format("{0,28:s}{1,0:s}", strTemp, m_End);
                            break;
                        default:
                            epcData = string.Format("{0,32:s}{1,0:s}", strTemp, m_End);
                            break;
                    }
                    epcData = epcData.Replace(" ", "0");

                    double datalenth = epcData.Length / 4.0;

                    if (num != datalenth)
                    {
                        MessageBox.Show("数据输入不规范", "提示");
                        return;
                    }

                    nB = nB + m_Count;

                    string chixA = "";

                    strTemp = "";

                    strTemp = (Convert.ToInt32(textBox11.Text, 16) + Convert.ToInt32(nB)).ToString("X");

                    epcData = epcData.ToUpper();

                    int nS = listView1.Items.Count;

                    chixA = string.Format("{0,0:d}", nS + 1);

                    ListViewItem item = new ListViewItem();

                    item = listView1.Items.Add((nS + 0).ToString(), chixA);
                    if (radioButton10.Checked)
                    {
                        item.SubItems.Add(epcData);
                        item.SubItems.Add("");
                        item.SubItems.Add("");
                    }
                    else if (radioButton12.Checked)
                    {
                        item.SubItems.Add("");
                        item.SubItems.Add(epcData);
                        item.SubItems.Add("");
                    }
                    else if (radioButton11.Checked)
                    {
                        item.SubItems.Add("");
                        item.SubItems.Add("");
                        item.SubItems.Add(epcData);
                    }
                }
            }

            if ((!m_CBefor) && (!m_CEnd))//前缀不打勾，后缀不打勾
            {
                listView1.Items.Clear();

                strTemp = textBox11.Text;//起始序号

                int nB = 0;
                for (i = 0; i < m_Sum; i++)//总数
                {
                    switch (m_Word)
                    {
                        case 1:
                            epcData = string.Format("{0,4:s}", strTemp);
                            break;
                        case 2:
                            epcData = string.Format("{0,8:s}", strTemp);
                            break;
                        case 3:
                            epcData = string.Format("{0,12:s}", strTemp);
                            break;
                        case 4:
                            epcData = string.Format("{0,16:s}", strTemp);
                            break;
                        case 5:
                            epcData = string.Format("{0,20:s}", strTemp);
                            break;
                        case 6:
                            epcData = string.Format("{0,24:s}", strTemp);
                            break;
                        case 7:
                            epcData = string.Format("{0,28:s}", strTemp);
                            break;
                        default:
                            epcData = string.Format("{0,32:s}", strTemp);
                            break;
                    }
                    epcData = epcData.Replace(" ", "0");

                    double datalenth = epcData.Length / 4.0;

                    if (num != datalenth)
                    {
                        MessageBox.Show("数据输入不规范", "提示");
                        return;
                    }

                    nB = nB + m_Count;

                    string chixA = "";

                    strTemp = (Convert.ToInt32(textBox11.Text, 16) + Convert.ToInt32(nB)).ToString("X");

                    epcData = epcData.ToUpper();

                    int nS = listView1.Items.Count;

                    chixA = string.Format("{0,0:d}", nS + 1);

                    ListViewItem item = new ListViewItem();

                    item = listView1.Items.Add((nS + 0).ToString(), chixA);
                    if (radioButton10.Checked)
                    {
                        item.SubItems.Add(epcData);
                        item.SubItems.Add("");
                        item.SubItems.Add("");
                    }
                    else if (radioButton12.Checked)
                    {
                        item.SubItems.Add("");
                        item.SubItems.Add(epcData);
                        item.SubItems.Add("");
                    }
                    else if (radioButton11.Checked)
                    {
                        item.SubItems.Add("");
                        item.SubItems.Add("");
                        item.SubItems.Add(epcData);
                    }
                }
            }
            textBox15.Text = m_Sum.ToString();      //总数
            textBox16.Text = "0";             //完成数

        }

        private void button8_Click(object sender, EventArgs e)
        {
            //开始发卡
            string str = "您确定要写EPC吗？";

            iFlagProtectOpt = 0;//0---不保护(不锁), 1---使用密码锁，2---永远锁住(死了), 3---EAS报警(add by martrin20140514)
            if (checkBox4.Checked || checkBox7.Checked) //使用密码写和EAS报警都打勾
            {
                byte[] chTemp = new byte[33];
                //int i = 0;
                int str_len = 0;
                string m_RWAccessPassword;

                m_RWAccessPassword = textBox20.Text;//密码
                iFlagProtectOpt = 1;//1---使用密码锁
                //访问密码
                str_len = m_RWAccessPassword.Length;
                if (str_len != 8)
                {
                    MessageBox.Show("Please input enough Length of AccessPassword!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                str = "您确定要使用密码写来锁定EPC吗？";
            }

            if (checkBox6.Checked)
            {
                iFlagProtectOpt = 2;//2---永远锁住(死了)

                str = "您确定要永远不可写来锁定EPC吗？";
            }

            if (checkBox7.Checked)
            {
                iFlagProtectOpt = 3;//3---EAS报警(add by martrin20140514)

                str = "您确定要EAS报警吗？";
            }


            if (0 != iFlagProtectOpt)
            {
                if (MessageBox.Show(str, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }

            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("待写入数据列表中没有数据!", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            label29.Text = listView1.Items[0].SubItems[1].Text;

            textBox21.Enabled = false;//间隔时间禁用
            button8.Enabled = false;//开始写卡
            button9.Enabled = true;//暂停写卡
            button10.Enabled = false;//移除当前数据了解
            button11.Enabled = false;//清空写入数据列表
            button15.Enabled = false;//按规则生成数据

            //使用EPC写要变不可用
            checkBox4.Enabled = false;
            textBox20.Enabled = false;
            checkBox6.Enabled = false;

            //开启线程
            nStop = true;
            tdThreadEPC = new Thread(new ThreadStart(this.ThreadEPC));
            tdThreadEPC.Start();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //暂停发卡
            nStop = false;
            checkBox8.Enabled = true;//写的EPC可用
            checkBox9.Enabled = true;//写的USE可用
            checkBox10.Enabled = true;//写的PASS可用
            textBox21.Enabled = true;//间隔时间可用
            button8.Enabled = true;//开始写卡
            button9.Enabled = false;//暂停写卡
            button10.Enabled = true;//移除当前数据了解
            button11.Enabled = true;//清空写入数据列表
            button15.Enabled = true;//按规则生成数据
            Thread.Sleep(800);
            tdThreadEPC.Abort();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //移除当前数据
            int icount = listView1.Items.Count;
            if (0 != icount)
            {
                listView1.Items.RemoveAt(0);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //清空列表数据
            listView1.Items.Clear();
        }

        /// <summary>
        /// 写日志文件的接口函数
        /// 例如:DllComm.TP_WriteAppLogFile("file.cs", 1109, "C:\\1.txt", 1, "ag45\n");
        /// </summary>
        /// <param name="FileName">源代码的当前源文件名</param>
        /// <param name="line">源代码的当前行号</param>
        /// <param name="DestFileName">要写入的日志全路径名</param>
        /// <param name="bOpt">类别:如果为1则将源文件名和当前行号一起写入到日志,否则不写</param>
        /// <param name="fmt">要写入的字符串信息</param>
        /// <returns>暂无作用</returns>
        public static int TP_WriteAppLogFile(string FileName, int line, string DestFileName, int bOpt, string fmt)
        {
            try
            {
                FileInfo file = new FileInfo(DestFileName);

                if (!file.Exists)
                {
                    file.Create();
                }

                //定位到文件尾
                StreamWriter stream = file.AppendText();

                //写当前的时间
                //stream.Write(DateTime.Now.ToString("【HH:mm:ss.fff】---"));

                //写用户传过来的字符串
                stream.WriteLine(fmt);

                //最后记着要关了它
                stream.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Log:" + e.Message.ToString());
            }

            return 0;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //生成成功数据TXT
            bool bFlag = false;
            string strText;
            int i = 0;
            int icount = listView2.Items.Count;
            if (0 == icount)
            {
                MessageBox.Show("列表中没有数据!", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = "TXT(*.txt)|*.txt|All Files(*.*)|*.*";

            DateTime CurrentTime = System.DateTime.Now;
            string strFile = CurrentTime.ToString("yyyy-MM-dd_HH_mm_ss") + ".txt";

            dialog.FileName = strFile;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (!File.Exists(dialog.FileName))//判断文件是否存在，否则创建
                {
                    FileStream fs1 = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1);
                    sw.Close();
                    fs1.Close();
                }

                for (i = 0; i < icount; i++)
                {
                    if (listView2.Items[i].SubItems[4].Text == "成功")
                    {
                        bFlag = true;
                        strText = "";
                        strText += listView2.Items[i].SubItems[0].Text;
                        strText += ",";
                        strText += listView2.Items[i].SubItems[1].Text;
                        strText += ",";
                        strText += listView2.Items[i].SubItems[2].Text;
                        strText += ",";
                        strText += listView2.Items[i].SubItems[3].Text;
                        strText += ",";
                        strText += listView2.Items[i].SubItems[4].Text;
                        TP_WriteAppLogFile("", 0, dialog.FileName, 0, strText);
                    }
                }
                if (bFlag)
                {
                    System.Diagnostics.Process.Start(dialog.FileName);
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //生成失败数据TXT 
            bool bFlag = false;
            string strText;
            int i = 0;
            int icount = listView2.Items.Count;
            if (0 == icount)
            {
                MessageBox.Show("列表中没有数据!", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = "TXT(*.txt)|*.txt|All Files(*.*)|*.*";

            DateTime CurrentTime = System.DateTime.Now;
            string strFile = CurrentTime.ToString("yyyy-MM-dd_HH_mm_ss") + ".txt";

            dialog.FileName = strFile;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (!File.Exists(dialog.FileName))//判断文件是否存在，否则创建
                {
                    FileStream fs1 = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1);
                    sw.Close();
                    fs1.Close();
                }

                for (i = 0; i < icount; i++)
                {
                    if (listView2.Items[i].SubItems[4].Text == "失败")
                    {
                        bFlag = true;
                        strText = "";
                        strText += listView2.Items[i].SubItems[0].Text;
                        strText += ",";
                        strText += listView2.Items[i].SubItems[1].Text;
                        strText += ",";
                        strText += listView2.Items[i].SubItems[2].Text;
                        strText += ",";
                        strText += listView2.Items[i].SubItems[3].Text;
                        strText += ",";
                        strText += listView2.Items[i].SubItems[4].Text;
                        TP_WriteAppLogFile("", 0, dialog.FileName, 0, strText);
                    }
                }
                if (bFlag)
                {
                    System.Diagnostics.Process.Start(dialog.FileName);
                }
            }
            return;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
        }

        private void ThreadEPC()
        {
            int t, j = 0;
            int i, k, nCounter = 0, ID_len = 0;// ID_len_temp = 0;
            string str, str_temp;
            byte[] IDBuffer = new byte[30 * 256];
            byte[] mask = new byte[512];
            byte[] IDTemp = new byte[12];
            int mem = -1;
            int EPC_Word;
            int len, ptr;
            byte[] AccessPassword = new byte[256];
            int ilock = 0;
            int Start_num = 0;
            int Next = 0;//判断是否继续向下执行0-否，1-是
            string EpcDatas = "";
            string ReEpc = "";

            while (true)
            {
                nStop = true;

                int iCounter = listView1.Items.Count;

                if (iCounter == 0)
                {
                    //列表数据完成或连接断开
                    nStop = false;
                    checkBox8.Enabled = true;//写的EPC可用
                    checkBox9.Enabled = true;//写的USE可用
                    checkBox10.Enabled = true;//写的PASS可用
                    textBox21.Enabled = true;//间隔时间可用
                    button8.Enabled = true;//开始写卡
                    button9.Enabled = false;//暂停写卡
                    button10.Enabled = true;//移除当前数据了解
                    button11.Enabled = true;//清空写入数据列表
                    button15.Enabled = true;//按规则生成数据
                    tdThreadEPC.Abort();
                    break;
                }

                if (0 == connect_OK)
                {
                    nStop = false;
                    checkBox8.Enabled = true;//写的EPC可用
                    checkBox9.Enabled = true;//写的USE可用
                    checkBox10.Enabled = true;//写的PASS可用
                    textBox21.Enabled = true;//间隔时间可用
                    button8.Enabled = true;//开始写卡
                    button9.Enabled = false;//暂停写卡
                    button10.Enabled = true;//移除当前数据了解
                    button11.Enabled = true;//清空写入数据列表
                    button15.Enabled = true;//按规则生成数据

                    break;
                }

                ptr = 0;
                len = 0;
                mem = 1;

                for (i = 0; i < 3; i++)
                {
                    res = Reader.EPC1G2_ReadLabelID(m_hScanner, mem, ptr, len, mask, IDBuffer, ref nCounter, RS485Address);

                    if (res == _OK)
                    {
                        break;
                    }
                }
                if (res == _OK)
                {
                    nStop = true;
                    for (i = 0; i < IDBuffer[0] * 2; i++)
                    {
                        IDTemp[i] = IDBuffer[i + 1];
                    }

                    for (i = 0; i < IDBuffer[0] * 2; i++)
                    {
                        ReEpc += IDBuffer[i + 1].ToString("X2");
                    }

                    if (EpcDatas != ReEpc)
                    {
                        nStop = true;
                    }
                    else
                    {
                        nStop = false;
                        panel1.BackColor = Color.Red;
                        label32.Text = "重复写卡，请更换另一张卡";
                        Thread.Sleep(300);
                    }
                }
                else
                {
                    nStop = false;
                }

                if (nStop)
                {
                    str = "00000000";

                    EpcDatas = ReEpc;

                    for (i = 0; i < 4; i++)
                    {
                        AccessPassWord[i] = (byte)Convert.ToInt16((str[i * 2].ToString() + str[i * 2 + 1].ToString()), 16);
                    }

                    str_temp = "";

                    if (checkBox10.Checked == true)
                    {
                        mem = 0;
                        //密码区
                        Start_num = 0;
                        EPC_Word = IDBuffer[0];

                        str_temp = listView1.Items[0].SubItems[2].Text;

                        bool isHexa = Regex.IsMatch(str_temp, "^[0-9A-Fa-f]+$");

                        if (!isHexa)
                        {
                            nStop = false;

                            panel1.BackColor = Color.Red;
                            label32.Text = "错误，待写入的数据错误";


                            break;
                        }

                        str = str_temp; //要写入的数据


                        for (i = 0; i < str.Length / 2; i++)
                        {
                            mask[i] = (byte)Convert.ToInt16((str[i * 2].ToString() + str[i * 2 + 1].ToString()), 16);
                        }

                        len = Convert.ToInt16(str_temp.Trim().Length / 4);

                        for (i = 0; i < 3; i++)
                        {
                            res = Reader.EPC1G2_WriteWordBlock(m_hScanner, Convert.ToByte(EPC_Word), IDTemp, Convert.ToByte(mem), Convert.ToByte(Start_num), Convert.ToByte(len), mask, AccessPassWord, RS485Address);

                            if ((res == _OK) || (res == 5) || (res == 8) || (res == 9))
                            {
                                break;
                            }
                        }
                    }

                    if (checkBox9.Checked == true)
                    {
                        mem = 3;
                        //user区
                        Start_num = 0;
                        EPC_Word = IDBuffer[0];

                        str_temp = listView1.Items[0].SubItems[3].Text;

                        bool isHexa = Regex.IsMatch(str_temp, "^[0-9A-Fa-f]+$");

                        if (!isHexa)
                        {
                            nStop = false;
                            panel1.BackColor = Color.Red;
                            label32.Text = "错误，待写入的数据错误";

                            break;
                        }

                        str = str_temp; //要写入的数据

                        for (i = 0; i < str.Length / 2; i++)
                        {
                            mask[i] = (byte)Convert.ToInt16((str[i * 2].ToString() + str[i * 2 + 1].ToString()), 16);
                        }

                        len = Convert.ToInt16(str_temp.Trim().Length / 4);

                        for (i = 0; i < 3; i++)
                        {
                            Thread.Sleep(20);

                            res = Reader.EPC1G2_WriteWordBlock(m_hScanner, Convert.ToByte(EPC_Word), IDTemp, Convert.ToByte(mem), Convert.ToByte(Start_num), Convert.ToByte(len), mask, AccessPassWord, RS485Address);

                            if ((res == _OK) || (res == 5) || (res == 8) || (res == 9))
                            {
                                break;
                            }
                        }
                    }

                    if (checkBox8.Checked == true)
                    {
                        mem = 1;
                        //EPC区
                        Start_num = 2;
                        EPC_Word = IDBuffer[0];

                        str_temp = listView1.Items[0].SubItems[1].Text;

                        bool isHexa = Regex.IsMatch(str_temp, "^[0-9A-Fa-f]+$");

                        if (!isHexa)
                        {
                            nStop = false;
                            panel1.BackColor = Color.Red;
                            label32.Text = "错误，待写入的数据错误";

                            break;
                        }

                        str = str_temp; //要写入的数据

                        EpcDatas = str;

                        for (i = 0; i < str.Length / 2; i++)
                        {
                            mask[i] = (byte)Convert.ToInt16((str[i * 2].ToString() + str[i * 2 + 1].ToString()), 16);
                        }

                        len = Convert.ToInt16(str_temp.Trim().Length / 4);

                        for (i = 0; i < 3; i++)
                        {
                            Thread.Sleep(20);

                            res = Reader.EPC1G2_WriteEPC(m_hScanner, (byte)len, mask, AccessPassword, RS485Address);
                            if ((res == _OK) || (res == 5) || (res == 8) || (res == 9))
                            {
                                break;
                            }
                        }
                    }
                    //这部分判断准备写入的数据是否正确
                    //取列表数据，并检查数据
                    if (res == _OK)
                    {
                        string stNo = listView1.Items[0].SubItems[0].Text;
                        string stEPC = listView1.Items[0].SubItems[1].Text;
                        string stPass = listView1.Items[0].SubItems[2].Text;
                        string stUse = listView1.Items[0].SubItems[3].Text;

                        int nRow = listView2.Items.Count;
                        ListViewItem item = new ListViewItem();
                        item = listView2.Items.Add((nRow + 0).ToString(), (nRow + 0).ToString());

                        if (checkBox8.Checked)
                        {
                            item.SubItems.Add(stEPC);
                        }
                        else
                        {
                            item.SubItems.Add(" ");
                        }

                        if (checkBox10.Checked)
                        {
                            item.SubItems.Add(stPass);
                        }
                        else
                        {
                            item.SubItems.Add(" ");
                        }

                        if (checkBox9.Checked)
                        {
                            item.SubItems.Add(stUse);
                        }
                        else
                        {
                            item.SubItems.Add(" ");
                        }

                        item.SubItems.Add("成功");

                        listView2.EnsureVisible(listView2.Items.Count - 1);

                        //界面显示信息
                        panel1.BackColor = Color.Green;
                        label32.Text = "写卡成功";

                        str = textBox19.Text;

                        k = 0;

                        int.TryParse(str, out k);//字符串转数字

                        k++;

                        textBox19.Text = k.ToString();

                        listView1.Items.RemoveAt(0);

                        if (listView1.Items.Count != 0)
                        {
                            string tmp = "";

                            if (radioButton10.Checked)
                            {
                                tmp = listView1.Items[0].SubItems[1].Text;
                            }
                            else if (radioButton12.Checked)
                            {
                                tmp = listView1.Items[0].SubItems[2].Text;
                            }
                            else if (radioButton11.Checked)
                            {
                                tmp = listView1.Items[0].SubItems[3].Text;
                            }

                            label29.Text = tmp;

                        }
                        else
                        {
                            label29.Text = "";
                        }
                        Next = 1;

                    }
                    else
                    {
                        panel1.BackColor = Color.Red;
                        label32.Text = string.Format("此卡未写入成功，请重新放入！");
                        Thread.Sleep(300);
                        Next = 0;
                    }

                    if (Next == 1)
                    {
                        int iSuccess = 0;//成功
                        if (iFlagProtectOpt == 1 || 2 == iFlagProtectOpt)  // 1使用密码写 2 永远锁住
                        {
                            for (int l = 0; l < 3; l++)
                            {
                                ptr = 0;
                                len = 0;
                                mem = 1;
                                res = Reader.EPC1G2_ReadLabelID(m_hScanner, mem, ptr, len, mask, IDBuffer, ref nCounter, RS485Address);

                                if (nCounter == 1)
                                {
                                    ID_len = IDBuffer[0];
                                    for (i = 0; i < ID_len * 2; i++)
                                    {
                                        IDTemp[i] = IDBuffer[i + 1];
                                    }
                                    break;
                                }
                                else
                                {
                                    Thread.Sleep(100);
                                }
                            }

                            //1---使用密码锁
                            len = 4;	//读取数据的长度
                            ptr = 0;	//起始地址
                            mem = 0;	//0--密码区,1-- EPC号,2-- TID标签ID号,3--用户区User
                            EPC_Word = ID_len;  //为EPC的长度
                            //////////////////////////////////////////////////////////////////////////
                            //第一步先读出原密码区
                            while (nStop)
                            {
                                for (i = 0; i < 512; i++)
                                {
                                    mask[i] = 0;
                                }

                                Thread.Sleep(30);
                                //EPC_Word 为EPC的长度
                                //IDTemp 传入的EPC号
                                //mem 0--密码区,1-- EPC号,2-- TID标签ID号,3--用户区User, 这里为0
                                //ptr 读取数据的起始地址, len 读取数据的长度
                                //mask读到的数据
                                //AccessPassword 原访问密码
                                res = Reader.EPC1G2_ReadWordBlock(m_hScanner, (byte)EPC_Word, IDTemp, (byte)mem, (byte)ptr, (byte)len, mask, AccessPassword, RS485Address);

                                if (res == _OK)
                                {
                                    //读到了原密码
                                    break;
                                }
                                else
                                {
                                    j++;
                                    t = res * 1000 + j;
                                    panel1.BackColor = Color.Red;
                                    label32.Text = string.Format("读原密码失败，请放入卡:{0,0:d}{1,0:s}{2,0:d}", t / 1000, "-", t % 1000);
                                    Thread.Sleep(50);
                                    nStop = false;
                                }
                            }
                            if (res != _OK)
                            {
                                iSuccess++;
                            }

                            //第2步先写新密码，用界面上的密码值epcDlg->AccessPassword
                            while (nStop)
                            {
                                len = 4;	//读取数据的长度
                                ptr = 0;	//掩码起始地址addr
                                mem = 0;	//0--密码区,1-- EPC号,2-- TID标签ID号,3--用户区User
                                EPC_Word = ID_len;//为EPC的长度
                                str = textBox20.Text;
                                for (i = 0; i < len; i++)
                                {
                                    mask[i] = (byte)Convert.ToInt16((str[i * 2].ToString() + str[i * 2 + 1].ToString()), 16);
                                }
                                for (i = 0; i < len; i++)
                                {
                                    mask[i + 4] = (byte)Convert.ToInt16((str[i * 2].ToString() + str[i * 2 + 1].ToString()), 16);
                                }

                                Thread.Sleep(30);

                                //EPC_Word 为EPC的长度
                                //IDTemp 传入的EPC号
                                //mem 0--密码区,1-- EPC号,2-- TID标签ID号,3--用户区User, 这里为0
                                //ptr 如果是密码=0, len =4
                                //这里是传的密码值 mask, 
                                //AccessPassword 原访问密码
                                res = Reader.EPC1G2_WriteWordBlock(m_hScanner, (byte)EPC_Word, IDTemp, (byte)mem, (byte)ptr, (byte)len, mask, AccessPassword, RS485Address);

                                if (res == _OK)
                                {
                                    break;
                                }
                                else
                                {
                                    j++;
                                    t = res * 1000 + j;
                                    panel1.BackColor = Color.Red;
                                    label32.Text = string.Format("写新密码失败，请放入卡:{0,0:d}{1,0:s}{2,0:d}", t / 1000, "-", t % 1000);
                                    Thread.Sleep(50);
                                    nStop = false;
                                }
                            }
                            if (res != _OK)
                            {
                                iSuccess++;
                            }

                            //第3步锁住EPC，用使用密码写来锁住EPC,注意传入的新密码
                            while (nStop)
                            {
                                EPC_Word = ID_len;//为EPC的长度
                                ptr = 0;	//掩码起始地址addr
                                mem = 2;	//0-- Kill Password 1--Access Password 2-- EPC号 3-- TID标签ID号 4--用户区User

                                //0--可写         1--永久可写
                                //2—带密码写     3--永不可写
                                //4--可读写       5--永久可读写
                                //6--带密码读写   7--永不可读写
                                //0～3只适用EPC、TID和User三个数据区；4～7只适用Kill Password和Access Password。
                                if (iFlagProtectOpt == 1)////0---不保护(不锁), 1---使用密码锁，2---永远锁住(死了), 3---EAS报警(add by martrin20140514)
                                {
                                    ilock = 2;//2—带密码写
                                }
                                else if (2 == iFlagProtectOpt)
                                {
                                    ilock = 3;//3-永不可写
                                }

                                for (i = 0; i < 8; i++) AccessPassword[i] = mask[i];

                                Thread.Sleep(30);

                                res = Reader.EPC1G2_SetLock(m_hScanner, (byte)EPC_Word, IDTemp, (byte)mem, (byte)ilock, AccessPassword, RS485Address);

                                if (res == _OK)
                                {
                                    break;
                                }
                                else
                                {
                                    j++;
                                    t = res * 1000 + j;
                                    panel1.BackColor = Color.Red;
                                    label32.Text = string.Format("锁EPC失败，请放入卡:{0,0:d}{1,0:s}{2,0:d}", t / 1000, "-", t % 1000);
                                    Thread.Sleep(50);
                                    nStop = false;
                                }
                            }
                            if (res != _OK)
                            {
                                iSuccess++;
                            }
                        }
                    }
                }

                ReEpc = "";
                //写入间隔时间
                str = textBox21.Text;
                k = 0;
                int.TryParse(str, out k);//字符串转数字

                if ((k < 0) || (k > 60))
                {
                    k = 1;
                }

                for (i = 0; i < k; i++)
                {
                    Thread.Sleep(1000);
                    panel1.BackColor = Color.Yellow;
                    label32.Text = string.Format("等待写卡，请放入卡");
                    nStop = true;
                }
            }
        }

        //private void ThreadEPC()
        //{
        //    int t = 0;

        //    int i, j, k, nCounter = 0, ID_len = 0;// ID_len_temp = 0;
        //    string str, str_temp;
        //    byte[] temp = new byte[512];
        //    byte[] IDBuffer = new byte[30 * 256];

        //    byte[] mask = new byte[512];
        //    byte[] IDTemp = new byte[512];
        //    int mem, ptr, len, EPC_Word;
        //    byte[] AccessPassword = new byte[32];

        //    byte[] p = new byte[3];

        //    int ilock = 0;

        //    Reader.VH_WriteAppLogFile("", 0, "ThreadEPC\r\n");

        //    while (nStop)
        //    {
        //        int iCounter = listView1.Items.Count;
        //        if (iCounter == 0)
        //        {
        //            //列表数据完成或连接断开
        //            nStop = false;
        //            checkBox8.Enabled = true;//写的EPC可用
        //            checkBox9.Enabled = true;//写的USE可用
        //            checkBox10.Enabled = true;//写的PASS可用
        //            textBox21.Enabled = true;//间隔时间可用
        //            button8.Enabled = true;//开始写卡
        //            button9.Enabled = false;//暂停写卡
        //            button10.Enabled = true;//移除当前数据了解
        //            button11.Enabled = true;//清空写入数据列表
        //            button15.Enabled = true;//按规则生成数据
        //            break;
        //        }

        //        if (0 == connect_OK)
        //        {
        //            nStop = false;
        //            checkBox8.Enabled = true;//写的EPC可用
        //            checkBox9.Enabled = true;//写的USE可用
        //            checkBox10.Enabled = true;//写的PASS可用
        //            textBox21.Enabled = true;//间隔时间可用
        //            button8.Enabled = true;//开始写卡
        //            button9.Enabled = false;//暂停写卡
        //            button10.Enabled = true;//移除当前数据了解
        //            button11.Enabled = true;//清空写入数据列表
        //            button15.Enabled = true;//按规则生成数据
        //        }

        //        if (checkBox8.Checked) //读卡中数据检测是否与上次写入数据相同，避免重复写卡
        //        {
        //            if (true)
        //            {
        //                len = 0;	//掩码长度LEN
        //                ptr = 0;	//掩码起始地址addr
        //                mem = 1;	//0--密码区,1-- EPC号,2-- TID标签ID号,3--用户区User
        //                ID_len = 0;
        //                j = 0;

        //                while (nStop)
        //                {
        //                    Thread.Sleep(30);
        //                    switch (ComMode)
        //                    {
        //                        case 0:
        //                            break;
        //                        case 1:
        //                            //读EPC号,nCounter为读的个数，被读的数据放在IDBuffer中
        //                            res = Reader.EPC1G2_ReadLabelID(m_hScanner, mem, ptr, len, mask, IDBuffer, ref nCounter, RS485Address);
        //                            break;
        //                        case 3://usb
        //                            //读EPC号,nCounter为读的个数，被读的数据放在IDBuffer中
        //                            res = Reader.EPC1G2_ReadLabelID(m_hScanner, mem, ptr, len, mask, IDBuffer, ref nCounter, 0);
        //                            break;
        //                    }
        //                    if (res == _OK)
        //                    {
        //                        string stroutput = string.Format("EPC1G2_ReadLabelID:OK({0,0:d})\r\n", nCounter);
        //                        Reader.VH_WriteAppLogFile("", 0, stroutput);
        //                        if (nCounter == 1)
        //                        {
        //                            ID_len = (int)IDBuffer[0];  //IDBuffer[0]为Word总数;1word=2Byte
        //                            for (i = 0; i < ID_len * 2; i++)
        //                            {
        //                                temp[i] = IDBuffer[i];
        //                            }

        //                            StringBuilder chTemp = new StringBuilder(512);
        //                            Reader.Bcd2AscEx(chTemp, temp, ID_len * 2 * 2);

        //                            stroutput = string.Format("EPC1G2_ReadLabelVl:({0,0:s})\r\n", chTemp);
        //                            Reader.VH_WriteAppLogFile("", 0, stroutput);

        //                            //去掉相同ID号，然后写卡，modify by mqs 20121010
        //                            bool bbEPCFlag = true;
        //                            for (i = 0; i < ID_len * 2; i++)
        //                            {
        //                                if (temp[i] != IDTemp[i])
        //                                {
        //                                    bbEPCFlag = false;
        //                                }
        //                            }

        //                            if (bbEPCFlag)
        //                            {
        //                                panel1.BackColor = Color.Red;
        //                                label32.Text = "重复写卡，请更换下一张卡";

        //                            }
        //                            else
        //                            {
        //                                panel1.BackColor = Color.Yellow;
        //                                label32.Text = "等待写卡或写卡失败，请放入卡:0-0";
        //                                break;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            panel1.BackColor = Color.Red;
        //                            label32.Text = "有多张卡存在，请放入一张卡";
        //                            Thread.Sleep(1000);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        j++;
        //                        t = res * 1000 + j;
        //                        panel1.BackColor = Color.Yellow;
        //                        label32.Text = string.Format("等待写卡或写卡失败，请放入卡:{0,0:d}{1,0:s}{2,0:d}", j / 1000, "-", j % 1000);
        //                        Thread.Sleep(300);
        //                    }
        //                }

        //            }

        //            //////////////////////////////////////////////////////////////////////////
        //            //这部分判断准备写入的数据是否正确
        //            //取列表数据，并检查数据
        //            str_temp = listView1.Items[0].SubItems[1].Text;
        //            str = str_temp;

        //            bool isHexa = Regex.IsMatch(str_temp, "^[0-9A-Fa-f]+$");

        //            if (!isHexa)
        //            {
        //                nStop = false;
        //                checkBox8.Enabled = true;//写的EPC可用
        //                checkBox9.Enabled = true;//写的USE可用
        //                checkBox10.Enabled = true;//写的PASS可用
        //                textBox21.Enabled = true;//间隔时间可用
        //                button8.Enabled = true;//开始写卡
        //                button9.Enabled = false;//暂停写卡
        //                button10.Enabled = true;//移除当前数据了解
        //                button11.Enabled = true;//清空写入数据列表
        //                button15.Enabled = true;//按规则生成数据

        //                panel1.BackColor = Color.Yellow;
        //                label32.Text = "错误，待写入的数据错误";

        //                MessageBox.Show("待写入的数据错误!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //            }

        //            j = 0;
        //            while (nStop) //写入数据的循环
        //            {
        //                for (i = 0; i < 512; i++) temp[i] = 0;
        //                for (i = 0; i < 512; i++) mask[i] = 0;
        //                for (i = 0; i < 512; i++) IDTemp[i] = 0;

        //                //设置写入的数据
        //                len = str.Length;
        //                for (i = 0; i < len / 2; i++)
        //                {
        //                    mask[i] = (byte)Convert.ToInt16((str[i * 2].ToString() + str[i * 2 + 1].ToString()), 16);
        //                }

        //                len = str.Length / 4;
        //                k = 0;
        //                for (i = 0; i < (len * 2); i++)
        //                {
        //                    k = k + 2;
        //                }
        //                for (i = 0; i < len * 2; i++) IDTemp[i] = mask[i];//IDTemp为准备写的数据

        //                Thread.Sleep(30);
        //                switch (ComMode)
        //                {
        //                    case 0:
        //                        break;
        //                    case 1:
        //                        res = Reader.EPC1G2_WriteEPC(m_hScanner, (byte)len, mask, AccessPassword, RS485Address);
        //                        break;
        //                    case 3://usb
        //                        res = Reader.EPC1G2_WriteEPC(m_hScanner, (byte)len, mask, AccessPassword, 0);
        //                        break;
        //                }

        //                if (res == _OK) //写完后立即读取，以便检测写入数据是否正确
        //                {
        //                    StringBuilder chTemp = new StringBuilder(512);
        //                    Reader.Bcd2AscEx(chTemp, mask, len * 2 * 2);

        //                    string stroutput = string.Format("EPC1G2_WriteEPC:OK({0,0:s})\r\n", chTemp);
        //                    Reader.VH_WriteAppLogFile("", 0, stroutput);

        //                    len = 0;
        //                    ptr = 0;
        //                    mem = 1;
        //                    for (i = 0; i < 512; i++) mask[i] = 0;//memset(mask,0,512);
        //                    for (i = 0; i < 512; i++) IDBuffer[i] = 0;
        //                    for (i = 0; i < 3; i++)
        //                    {
        //                        Thread.Sleep(30);
        //                        switch (ComMode)
        //                        {
        //                            case 0:
        //                                break;
        //                            case 1:
        //                                res = Reader.EPC1G2_ReadLabelID(m_hScanner, mem, ptr, len, mask, IDBuffer, ref nCounter, RS485Address);
        //                                break;
        //                            case 3://usb
        //                                res = Reader.EPC1G2_ReadLabelID(m_hScanner, mem, ptr, len, mask, IDBuffer, ref nCounter, 0);
        //                                break;
        //                        }
        //                        if (_OK == res)
        //                        {
        //                            break;
        //                        }
        //                    }
        //                    ID_len = (int)IDBuffer[0];  //IDBuffer[0]为Word总数;1word=2Byte

        //                    for (i = 0; i < ID_len * 2; i++) temp[i] = IDBuffer[i + 1];//memcpy(temp,&IDBuffer[1],ID_len*2);//temp为读到的数据，

        //                    bool bbEPCFlag = true;
        //                    for (i = 0; i < ID_len * 2; i++)
        //                    {
        //                        if (temp[i] != IDTemp[i])
        //                        {
        //                            bbEPCFlag = false;
        //                            break;
        //                        }
        //                    }
        //                    if (bbEPCFlag)
        //                    {
        //                        //到这里来，说明写成功了
        //                        StringBuilder chTempx = new StringBuilder(512);
        //                        Reader.Bcd2AscEx(chTempx, temp, len * 2);

        //                        string stroutputx = string.Format("EPC1G2_ReadLabelIE:OK({0,0:s})\r\n", chTempx);
        //                        Reader.VH_WriteAppLogFile("", 0, stroutputx);


        //                        //////////////////////////////////////////////////////////////////////////
        //                        //if(epcDlg->m_SetUse)//写入用户区数据
        //                        //{
        //                        //    //取列表数据，并检查数据
        //                        //    str_temp=epcDlg->m_ListInData.GetItemText(0,2); 

        //                        //    str=str_temp.SpanIncluding(_T("0123456789ABCDEFabcdef"));

        //                        //    if(str.GetLength()!=str_temp.GetLength())
        //                        //    {
        //                        //        SendMessage(epcDlg->m_hWnd,WM_MSG,100,NULL);//待写入数据错误
        //                        //        epcDlg->nStop=0;
        //                        //        break;
        //                        //    }

        //                        //    memset(temp,0,512);	memset(mask,0,512);	

        //                        //    //设置写入的数据
        //                        //    //WideCharToMultiByte( CP_ACP, 0, str.GetBuffer(), -1,(char *)temp,str.GetLength()*2,NULL,NULL );
        //                        //    len=str.GetLength();
        //                        //    memcpy(temp, str, len);

        //                        //    len=str.GetLength()/4;
        //                        //    k=0;
        //                        //    for(i=0;i<(len*2);i++)
        //                        //    {
        //                        //        memset(p,0,3);
        //                        //        memcpy(p,&temp[k],2);
        //                        //        k=k+2;
        //                        //        mask[i]=(BYTE)strtol(&p[0],NULL,16);
        //                        //    }

        //                        //    EPC_Word=ID_len;  mem=3; ptr=0; j=0;
        //                        //    memset(AccessPassword,0,4);

        //                        //    while(epcDlg->nStop)
        //                        //    {
        //                        //        Sleep(30);
        //                        //        switch(ConnectMode)
        //                        //        {
        //                        //        case 0:		
        //                        //            break;
        //                        //        case 1:
        //                        //            res=EPC1G2_WriteWordBlock(m_hScanner,EPC_Word,IDTemp,mem,ptr,len,mask,AccessPassword,RS485Address);
        //                        //            break;
        //                        //        case 3://usb
        //                        //            res=EPC1G2_WriteWordBlock(m_hScanner,EPC_Word,IDTemp,mem,ptr,len,mask,AccessPassword,0);
        //                        //            break;
        //                        //        }

        //                        //        if(res==_OK)
        //                        //        {
        //                        //            break;
        //                        //        }
        //                        //        else
        //                        //        {
        //                        //            j++;
        //                        //            t=res*1000+j;
        //                        //            SendMessage(epcDlg->m_hWnd,WM_MSG,103,t);//用户区数据写入失败，请放入卡:
        //                        //            Sleep(1000);
        //                        //        }
        //                        //    }
        //                        //}

        //                        //if(epcDlg->m_SetPass)//写入密码区数据
        //                        //{
        //                        //    //取列表数据，并检查数据
        //                        //    str_temp=epcDlg->m_ListInData.GetItemText(0,3); 

        //                        //    str=str_temp.SpanIncluding(_T("0123456789ABCDEFabcdef"));

        //                        //    if(str.GetLength()!=str_temp.GetLength())
        //                        //    {
        //                        //        SendMessage(epcDlg->m_hWnd,WM_MSG,100,NULL);//待写入数据错误
        //                        //        epcDlg->nStop=0;
        //                        //        break;
        //                        //    }

        //                        //    memset(temp,0,512);	memset(mask,0,512);	

        //                        //    //设置写入的数据
        //                        //    //WideCharToMultiByte( CP_ACP, 0, str.GetBuffer(), -1,(char *)temp,str.GetLength()*2,NULL,NULL );
        //                        //    len=str.GetLength();
        //                        //    memcpy(temp, str, len);

        //                        //    len=str.GetLength()/4;
        //                        //    k=0;
        //                        //    for(i=0;i<(len*2);i++)
        //                        //    {
        //                        //        memset(p,0,3);
        //                        //        memcpy(p,&temp[k],2);
        //                        //        k=k+2;
        //                        //        mask[i]=(BYTE)strtol(&p[0],NULL,16);
        //                        //    }

        //                        //    EPC_Word=ID_len;  mem=0; ptr=0; j=0;
        //                        //    memset(AccessPassword,0,4);

        //                        //    while(epcDlg->nStop)
        //                        //    {
        //                        //        Sleep(30);
        //                        //        switch(ConnectMode)
        //                        //        {
        //                        //        case 0:		
        //                        //            break;
        //                        //        case 1:
        //                        //            res=EPC1G2_WriteWordBlock(m_hScanner,EPC_Word,IDTemp,mem,ptr,len,mask,AccessPassword,RS485Address);
        //                        //            break;
        //                        //        case 3://usb
        //                        //            res=EPC1G2_WriteWordBlock(m_hScanner,EPC_Word,IDTemp,mem,ptr,len,mask,AccessPassword,0);
        //                        //            break;
        //                        //        }

        //                        //        if(res==_OK)
        //                        //        {
        //                        //            break;
        //                        //        }
        //                        //        else
        //                        //        {
        //                        //            j++;
        //                        //            t=res*1000+j;
        //                        //            SendMessage(epcDlg->m_hWnd,WM_MSG,104,t);//密码区数据写入失败，请放入卡:%d-%d
        //                        //            Sleep(1000);
        //                        //        }
        //                        //    }
        //                        //}


        //                        //////////////////////////////////////////////////////////////////////////
        //                        //add by mqs 20130409 增加EPC保护功能:
        //                        //1.使用密码写保护(将来有一天要还原，可以找回)
        //                        //2.永远不可写，这种作法跟死了没啥区别，相当于挂了一样，只能以后换标签。
        //                        //iFlagProtectOpt	=	0;//0---不保护(不锁), 1---使用密码锁，2---永远锁住(死了), 3---EAS报警(add by martrin20140514)
        //                        int iSuccess = 0;//成功
        //                        if (iFlagProtectOpt == 1 || 2 == iFlagProtectOpt)
        //                        {
        //                            //1---使用密码锁
        //                            len = 4;	//读取数据的长度
        //                            ptr = 0;	//
        //                            mem = 0;	//0--密码区,1-- EPC号,2-- TID标签ID号,3--用户区User
        //                            EPC_Word = ID_len;//为EPC的长度1

        //                            //////////////////////////////////////////////////////////////////////////
        //                            //第一步先读出原密码区
        //                            while (nStop)
        //                            {

        //                                for (i = 0; i < 512; i++) mask[i] = 0;//memset(mask,0,512);
        //                                Thread.Sleep(30);

        //                                switch (ComMode)
        //                                {
        //                                    case 0:
        //                                        break;
        //                                    case 1:
        //                                    case 3://usb
        //                                        //EPC_Word 为EPC的长度
        //                                        //IDTemp 传入的EPC号
        //                                        //mem 0--密码区,1-- EPC号,2-- TID标签ID号,3--用户区User, 这里为0
        //                                        //ptr 读取数据的起始地址, len 读取数据的长度
        //                                        //读到的东东放在这里 mask, 
        //                                        //AccessPassword 原访问密码
        //                                        res = Reader.EPC1G2_ReadWordBlock(m_hScanner, (byte)EPC_Word, IDTemp, (byte)mem, (byte)ptr, (byte)len, mask, AccessPassword, RS485Address);
        //                                        break;
        //                                }

        //                                if (res == _OK)
        //                                {
        //                                    //读到了原密码
        //                                    StringBuilder chTempy = new StringBuilder(512);
        //                                    Reader.Bcd2AscEx(chTempy, mask, len * 2);

        //                                    string stroutputy = string.Format("EPC1G2_ReadWordBlock:ReadPassword({0,0:s})\r\n", chTempy);
        //                                    Reader.VH_WriteAppLogFile("", 0, stroutputy);
        //                                    break;
        //                                }
        //                                else
        //                                {
        //                                    j++;
        //                                    t = res * 1000 + j;

        //                                    panel1.BackColor = Color.Red;

        //                                    label32.Text = string.Format("读原密码失败，请放入卡:{0,0:d}{1,0:s}{2,0:d}", t / 1000, "-", t % 1000);
        //                                    Thread.Sleep(300);
        //                                }
        //                            }
        //                            if (res != _OK)
        //                            {
        //                                iSuccess++;
        //                            }

        //                            //第2步先写新密码，用界面上的密码值epcDlg->AccessPassword
        //                            while (nStop)
        //                            {
        //                                len = 4;	//读取数据的长度
        //                                ptr = 0;	//掩码起始地址addr
        //                                mem = 0;	//0--密码区,1-- EPC号,2-- TID标签ID号,3--用户区User
        //                                EPC_Word = ID_len;//为EPC的长度
        //                                str = textBox20.Text;
        //                                for (i = 0; i < len; i++)
        //                                {
        //                                    mask[i] = (byte)Convert.ToInt16((str[i * 2].ToString() + str[i * 2 + 1].ToString()), 16);
        //                                }
        //                                for (i = 0; i < len; i++)
        //                                {
        //                                    mask[i + 4] = (byte)Convert.ToInt16((str[i * 2].ToString() + str[i * 2 + 1].ToString()), 16);
        //                                }

        //                                Thread.Sleep(30);

        //                                switch (ComMode)
        //                                {
        //                                    case 0:
        //                                        break;
        //                                    case 1:
        //                                    case 3://usb
        //                                        //EPC_Word 为EPC的长度
        //                                        //IDTemp 传入的EPC号
        //                                        //mem 0--密码区,1-- EPC号,2-- TID标签ID号,3--用户区User, 这里为0
        //                                        //ptr 如果是密码=0, len =4
        //                                        //这里是传的密码值 mask, 
        //                                        //AccessPassword 原访问密码
        //                                        res = Reader.EPC1G2_WriteWordBlock(m_hScanner, (byte)EPC_Word, IDTemp, (byte)mem, (byte)ptr, (byte)len, mask, AccessPassword, RS485Address);
        //                                        break;
        //                                }

        //                                if (res == _OK)
        //                                {
        //                                    StringBuilder chTempy = new StringBuilder(512);
        //                                    Reader.Bcd2AscEx(chTempy, mask, len * 2);

        //                                    string stroutputy = string.Format("EPC1G2_ReadWordBlock:New Password({0,0:s})\r\n", chTempy);
        //                                    Reader.VH_WriteAppLogFile("", 0, stroutputy);
        //                                }
        //                                else
        //                                {
        //                                    j++;
        //                                    t = res * 1000 + j;
        //                                    //写新密码失败，请放入卡，请放入卡:
        //                                    panel1.BackColor = Color.Red;

        //                                    label32.Text = string.Format("写新密码失败，请放入卡:{0,0:d}{1,0:s}{2,0:d}", t / 1000, "-", t % 1000);
        //                                    Thread.Sleep(300);
        //                                }
        //                            }
        //                            if (res != _OK)
        //                            {
        //                                iSuccess++;
        //                            }
        //                            //////////////////////////////////////////////////////////////////////////
        //                            //第3步锁住EPC，用使用密码写来锁住EPC,注意传入的新密码
        //                            while (nStop)
        //                            {
        //                                EPC_Word = ID_len;//为EPC的长度
        //                                ptr = 0;	//掩码起始地址addr
        //                                mem = 2;	//0-- Kill Password 1--Access Password 2-- EPC号 3-- TID标签ID号 4--用户区User

        //                                //0--可写         1--永久可写
        //                                //2—带密码写     3--永不可写
        //                                //4--可读写       5--永久可读写
        //                                //6--带密码读写   7--永不可读写
        //                                //0～3只适用EPC、TID和User三个数据区；4～7只适用Kill Password和Access Password。
        //                                if (iFlagProtectOpt == 1)////0---不保护(不锁), 1---使用密码锁，2---永远锁住(死了), 3---EAS报警(add by martrin20140514)
        //                                {
        //                                    ilock = 2;//2—带密码写
        //                                }
        //                                else if (2 == iFlagProtectOpt)
        //                                {
        //                                    ilock = 3;//3-永不可写
        //                                }

        //                                for (i = 0; i < 8; i++) AccessPassword[i] = mask[i];//memcpy(AccessPassword, mask, 8);

        //                                Thread.Sleep(30);

        //                                switch (ComMode)
        //                                {
        //                                    case 0:
        //                                        break;
        //                                    case 1:
        //                                    case 3://usb
        //                                        res = Reader.EPC1G2_SetLock(m_hScanner, (byte)EPC_Word, IDTemp, (byte)mem, (byte)ilock, AccessPassword, RS485Address);
        //                                        break;
        //                                }

        //                                if (res == _OK)
        //                                {
        //                                    string stroutputy = string.Format("EPC1G2_SetLock:EPC Lock({0,0:d})\r\n", ilock);
        //                                    Reader.VH_WriteAppLogFile("", 0, stroutputy);
        //                                }
        //                                else
        //                                {
        //                                    j++;
        //                                    t = res * 1000 + j;
        //                                    //锁EPC数据写入失败，请放入卡:
        //                                    panel1.BackColor = Color.Red;

        //                                    label32.Text = string.Format("锁EPC失败，请放入卡:{0,0:d}{1,0:s}{2,0:d}", t / 1000, "-", t % 1000);
        //                                    Thread.Sleep(300);
        //                                }
        //                            }
        //                            if (res != _OK)
        //                            {
        //                                iSuccess++;
        //                            }
        //                            //////////////////////////////////////////////////////////////////////////
        //                            //第4步要进行密码保护，使用密码可读写来保护
        //                            while (nStop)
        //                            {
        //                                EPC_Word = ID_len;//为EPC的长度
        //                                ptr = 0;	//掩码起始地址addr
        //                                mem = 1;	//0-- Kill Password 1--Access Password 2-- EPC号 3-- TID标签ID号 4--用户区User

        //                                //0--可写         1--永久可写
        //                                //2—带密码写     3--永不可写
        //                                //4--可读写       5--永久可读写
        //                                //6--带密码读写   7--永不可读写
        //                                //0～3只适用EPC、TID和User三个数据区；4～7只适用Kill Password和Access Password。
        //                                if (iFlagProtectOpt == 1)////0---不保护(不锁), 1---使用密码锁，2---永远锁住(死了), 3---EAS报警(add by martrin20140514)
        //                                {
        //                                    ilock = 6;//6—带密码读写
        //                                }
        //                                else if (2 == iFlagProtectOpt)
        //                                {
        //                                    ilock = 7;//7--永不可读写
        //                                }
        //                                Thread.Sleep(30);

        //                                switch (ComMode)
        //                                {
        //                                    case 0:
        //                                        break;
        //                                    case 1:
        //                                    case 3://usb
        //                                        //EPC_Word 为EPC的长度
        //                                        //IDTemp 传入的EPC号
        //                                        //mem 0--密码区,1-- EPC号,2-- TID标签ID号,3--用户区User, 这里为0
        //                                        //ptr 读取数据的起始地址, len 读取数据的长度
        //                                        //读到的东东放在这里 mask, 
        //                                        //AccessPassword 原访问密码
        //                                        res = Reader.EPC1G2_SetLock(m_hScanner, (byte)EPC_Word, IDTemp, (byte)mem, (byte)ilock, AccessPassword, RS485Address);
        //                                        break;
        //                                }

        //                                if (res == _OK)
        //                                {
        //                                    string stroutputy = string.Format("EPC1G2_SetLock:PasswordLock({0,0:d})\r\n", ilock);
        //                                    Reader.VH_WriteAppLogFile("", 0, stroutputy);
        //                                }
        //                                else
        //                                {
        //                                    j++;
        //                                    t = res * 1000 + j;
        //                                    //锁密码失败，请放入卡:
        //                                    panel1.BackColor = Color.Red;

        //                                    label32.Text = string.Format("锁密码失败，请放入卡:{0,0:d}{1,0:s}{2,0:d}", t / 1000, "-", t % 1000);
        //                                    Thread.Sleep(300);
        //                                }
        //                            }
        //                            if (res != _OK)
        //                            {
        //                                iSuccess++;
        //                            }
        //                            //////////////////////////////////////////////////////////////////////////
        //                            //到止为止才算锁住EPC，要想解开必须知道密码。
        //                        }

        //                        //////////////////////////////////////////////////////////////////////////
        //                        //add by mqs 20140514 增加EAS功能:
        //                        //1.使用密码写保护(将来有一天要还原，可以找回)
        //                        //2.永远不可写，这种作法跟死了没啥区别，相当于挂了一样，只能以后换标签。
        //                        //iFlagProtectOpt	=	0;//0---不保护(不锁), 1---使用密码锁，2---永远锁住(死了), 3---EAS报警(add by martrin20140514)
        //                        //int iSuccess = 0 ;//成功
        //                        if (iFlagProtectOpt == 3)
        //                        {
        //                            //////////////////////////////////////////////////////////////////////////
        //                            //对了EAS报警来说，一般两步可以完成
        //                            //第1步先写新密码，用界面上的密码值epcDlg->AccessPassword
        //                            while (nStop)
        //                            {
        //                                len = 4;	//读取数据的长度
        //                                ptr = 0;	//掩码起始地址addr
        //                                mem = 0;	//0--密码区,1-- EPC号,2-- TID标签ID号,3--用户区User
        //                                EPC_Word = ID_len;//为EPC的长度

        //                                str = textBox20.Text;
        //                                for (i = 0; i < len; i++)
        //                                {
        //                                    mask[i] = (byte)Convert.ToInt16((str[i * 2].ToString() + str[i * 2 + 1].ToString()), 16);
        //                                }
        //                                for (i = 0; i < len; i++)
        //                                {
        //                                    mask[i + 4] = (byte)Convert.ToInt16((str[i * 2].ToString() + str[i * 2 + 1].ToString()), 16);
        //                                }

        //                                Thread.Sleep(30);

        //                                switch (ComMode)
        //                                {
        //                                    case 0:
        //                                        break;
        //                                    case 1:
        //                                    case 3://usb
        //                                        //EPC_Word 为EPC的长度
        //                                        //IDTemp 传入的EPC号
        //                                        //mem 0--密码区,1-- EPC号,2-- TID标签ID号,3--用户区User, 这里为0
        //                                        //ptr 如果是密码=0, len =4
        //                                        //这里是传的密码值 mask, 
        //                                        //AccessPassword 原访问密码
        //                                        res = Reader.EPC1G2_WriteWordBlock(m_hScanner, (byte)EPC_Word, IDTemp, (byte)mem, (byte)ptr, (byte)len, mask, AccessPassword, RS485Address);
        //                                        break;
        //                                }

        //                                if (res == _OK)
        //                                {
        //                                    StringBuilder chTempy = new StringBuilder(512);
        //                                    Reader.Bcd2AscEx(chTempy, mask, len * 2 * 2);

        //                                    string stroutputy = string.Format("EPC1G2_ReadWordBlock:New Password({0,0:s})\r\n", chTempy);
        //                                    Reader.VH_WriteAppLogFile("", 0, stroutputy);
        //                                }
        //                                else
        //                                {
        //                                    j++;
        //                                    t = res * 1000 + j;
        //                                    //写新密码失败，请放入卡，请放入卡:
        //                                    panel1.BackColor = Color.Red;

        //                                    label32.Text = string.Format("写新密码失败，请放入卡:{0,0:d}{1,0:s}{2,0:d}", t / 1000, "-", t % 1000);
        //                                    Thread.Sleep(300);
        //                                }
        //                            }
        //                            if (res != _OK)
        //                            {
        //                                iSuccess++;
        //                            }
        //                            //////////////////////////////////////////////////////////////////////////
        //                            //第2步设置EAS报警位
        //                            while (nStop)
        //                            {
        //                                len = 4;	//读取数据的长度
        //                                ptr = 0;	//掩码起始地址addr
        //                                mem = 0;	//0--密码区,1-- EPC号,2-- TID标签ID号,3--用户区User
        //                                EPC_Word = ID_len;//为EPC的长度
        //                                str = textBox20.Text;
        //                                for (i = 0; i < len; i++)
        //                                {
        //                                    mask[i] = (byte)Convert.ToInt16((str[i * 2].ToString() + str[i * 2 + 1].ToString()), 16);
        //                                }
        //                                for (i = 0; i < len; i++)
        //                                {
        //                                    mask[i + 4] = (byte)Convert.ToInt16((str[i * 2].ToString() + str[i * 2 + 1].ToString()), 16);
        //                                }

        //                                Thread.Sleep(30);

        //                                switch (ComMode)
        //                                {
        //                                    case 0:
        //                                        break;
        //                                    case 1:
        //                                    case 3://usb
        //                                        //EPC_Word 为EPC的长度
        //                                        //IDTemp 传入的EPC号
        //                                        //EASstate	1个字节	0-不报警 1-报警,这里指1
        //                                        //AccessPassword 原访问密码 
        //                                        //res=EPC1G2_ChangeEas(m_hScanner,EPC_Word,IDTemp, 1, AccessPassword, RS485Address);
        //                                        res = Reader.EPC1G2_ChangeEas(m_hScanner, (byte)EPC_Word, IDTemp, (byte)1, mask, RS485Address);
        //                                        break;
        //                                }

        //                                if (res == _OK)
        //                                {
        //                                    string stroutputy = "EPC1G2_ChangeEas is 1!\r\n";
        //                                    Reader.VH_WriteAppLogFile("", 0, stroutputy);
        //                                }
        //                                else
        //                                {
        //                                    j++;
        //                                    t = res * 1000 + j;
        //                                    panel1.BackColor = Color.Red;

        //                                    label32.Text = string.Format("设置EAS报警失败，请放入卡:{0,0:d}{1,0:s}{2,0:d}", t / 1000, "-", t % 1000);
        //                                    Thread.Sleep(300);
        //                                }
        //                            }
        //                            if (res != _OK)
        //                            {
        //                                iSuccess++;
        //                            }
        //                        }

        //                        //到这里来写成功了
        //                        //记录当前的写入数据
        //                        string stNo = listView1.Items[0].SubItems[0].Text;
        //                        string stEPC = listView1.Items[0].SubItems[1].Text;
        //                        string stUse = listView1.Items[0].SubItems[2].Text;
        //                        string stPass = listView1.Items[0].SubItems[3].Text;
        //                        //增加完成列表

        //                        int nRow = listView2.Items.Count;
        //                        ListViewItem item = new ListViewItem();
        //                        item = listView2.Items.Add((nRow + 0).ToString(), (nRow + 0).ToString());
        //                        item.SubItems.Add(stEPC);
        //                        item.SubItems.Add(stUse);
        //                        item.SubItems.Add(stPass);
        //                        item.SubItems.Add("成功");

        //                        //界面显示信息
        //                        string tmp = listView1.Items[0].SubItems[1].Text;
        //                        label29.Text = tmp;
        //                        panel1.BackColor = Color.Green;
        //                        label32.Text = "写卡成功，请移开此卡!";

        //                        label33.Text = (stEPC);
        //                        //完成数
        //                        str = textBox16.Text;
        //                        k = 0;
        //                        int.TryParse(str, out k);//字符串转数字
        //                        k++;
        //                        textBox16.Text = k.ToString();

        //                        //剩余数
        //                        str = textBox17.Text;
        //                        k = 0;
        //                        int.TryParse(str, out k);//字符串转数字
        //                        k--;
        //                        textBox17.Text = k.ToString();

        //                        //正确发卡数目
        //                        str = textBox19.Text;
        //                        k = 0;
        //                        int.TryParse(str, out k);//字符串转数字
        //                        k++;
        //                        textBox19.Text = k.ToString();
        //                        listView1.Items.RemoveAt(0);
        //                        break;
        //                    }
        //                    else
        //                    {
        //                        //此卡未写入成功，请重新放入
        //                        panel1.BackColor = Color.Red;
        //                        label32.Text = "此卡未写入成功，请重新放入";
        //                        Thread.Sleep(1000);
        //                    }
        //                }
        //                else
        //                {
        //                    j++;
        //                    t = res * 1000 + j;
        //                    //等待写卡，请放入卡
        //                    panel1.BackColor = Color.Yellow;
        //                    label32.Text = string.Format("等待写卡或写卡失败，请放入卡:{0,0:d}{1,0:s}{2,0:d}", j / 1000, "-", j % 1000);
        //                    Thread.Sleep(100);
        //                }
        //            }
        //        }

        //        //写入间隔时间
        //        str = textBox21.Text;
        //        k = 0;
        //        int.TryParse(str, out k);//字符串转数字
        //        if ((k <= 0) || (k > 60))
        //        {
        //            k = 1;
        //        }
        //        for (i = 0; i < (int)k; i++)
        //        {
        //            Thread.Sleep(1000);
        //            //等待写卡，请放入卡,i指的是时间
        //            panel1.BackColor = Color.Yellow;
        //            label32.Text = string.Format("等待写卡，请放入卡:{0,0:d}", i);
        //        }
        //    }
        //}

        //============ISO18000-6C-页面结束============================
        #endregion

        #region ISO18000-6B----页面

        private void button15_Click(object sender, EventArgs e)
        {
            //移除当前数据
            if (listView3.Items.Count != 0)
            {
                listView3.Items.RemoveAt(0);
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            listView3.Items.Clear();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            //保存TXT
            bool bFlag = false;
            string strText;
            int i = 0;
            int icount = listView3.Items.Count;
            if (0 == icount)
            {
                MessageBox.Show("列表中没有数据!", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = "TXT(*.txt)|*.txt|All Files(*.*)|*.*";

            DateTime CurrentTime = System.DateTime.Now;
            string strFile = CurrentTime.ToString("yyyy-MM-dd_HH_mm_ss") + ".txt";

            dialog.FileName = strFile;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (!File.Exists(dialog.FileName))//判断文件是否存在，否则创建
                {
                    FileStream fs1 = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1);
                    //开始写入值
                    sw.Close();
                    fs1.Close();
                }

                for (i = 0; i < icount; i++)
                {
                    bFlag = true;
                    strText = "";
                    strText += listView3.Items[i].SubItems[0].Text;
                    strText += ",";
                    strText += listView3.Items[i].SubItems[1].Text;
                    strText += ",";
                    strText += listView3.Items[i].SubItems[2].Text;
                    strText += ",";
                    strText += listView3.Items[i].SubItems[3].Text;
                    strText += ",";
                    strText += listView3.Items[i].SubItems[4].Text;
                    TP_WriteAppLogFile("", 0, dialog.FileName, 0, strText);
                }
                if (bFlag)
                {
                    System.Diagnostics.Process.Start(dialog.FileName);
                }
            }
            return;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            //保存XLS
            //导出EXCEL
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = "Excel(*.xls)|*.xls|All Files(*.*)|*.*";

            DateTime CurrentTime = System.DateTime.Now;
            string strFile = CurrentTime.ToString("yyyy-MM-dd_HH_mm_ss") + ".xls";

            dialog.FileName = strFile;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                object missing = System.Reflection.Missing.Value;
                try
                {
                    if (xlApp == null)
                    {
                        MessageBox.Show("无法创建Excel对象，可能您的机子未安装Excel");
                        return;
                    }

                    Microsoft.Office.Interop.Excel.Workbooks xlBooks = xlApp.Workbooks;
                    Microsoft.Office.Interop.Excel.Workbook xlBook = xlBooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                    Microsoft.Office.Interop.Excel.Worksheet xlSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlBook.Worksheets[1];
                    Microsoft.Office.Interop.Excel.Range range = null;
                    xlSheet.Name = "导出信息";
                    //****** 抬头 ********************************************************************************* 

                    range = xlSheet.get_Range("A1", "F1");
                    range.Merge(Missing.Value);         // 合并单元格 
                    range.Columns.AutoFit();            // 设置列宽为自动适应                   
                    // 设置单元格左边框加粗 
                    range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThick;
                    // 设置单元格右边框加粗 
                    range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThick;
                    range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;// 设置单元格水平居中
                    range.Value2 = "发卡机导出信息";
                    range.Font.Size = 18;                        // 设置字体大小 
                    range.Font.ColorIndex = 5;                  // 设置字体颜色  
                    range.Font.Bold = true;
                    //range.Interior.ColorIndex = 6;  // 设置单元格背景色 
                    range.RowHeight = 28;           // 设置行高 
                    range.ColumnWidth = 30;         // 设置列宽 

                    xlSheet.Cells[2, 1] = "NO.";
                    xlSheet.Cells[2, 2] = "EPC";
                    xlSheet.Cells[2, 3] = "User";
                    xlSheet.Cells[2, 4] = "TID";
                    xlSheet.Cells[2, 5] = "Password";

                    int rowIndex = 3;//这个用来标记数据有多少行位置

                    //-----------------------设置单元格--------------------------------------------------------------------------------
                    range = xlSheet.get_Range(xlSheet.Cells[3, 1], xlSheet.Cells[rowIndex + this.listView3.Items.Count, 1]); //
                    range.NumberFormatLocal = "@";//文本格式
                    range.ColumnWidth = 5; //"序号";

                    range = xlSheet.get_Range(xlSheet.Cells[3, 2], xlSheet.Cells[rowIndex + this.listView3.Items.Count, 2]); //
                    range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;// 设置单元格水平居左
                    range.NumberFormatLocal = "@";//文本格式
                    range.ColumnWidth = 26; //"布卷号";

                    range = xlSheet.get_Range(xlSheet.Cells[3, 3], xlSheet.Cells[rowIndex + this.listView3.Items.Count, 3]); //
                    range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;// 设置单元格水平居左
                    range.NumberFormatLocal = "@";//文本格式
                    range.ColumnWidth = 26; //"地台板号";

                    range = xlSheet.get_Range(xlSheet.Cells[3, 4], xlSheet.Cells[rowIndex + this.listView3.Items.Count, 4]); //
                    range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;// 设置单元格水平居左
                    range.NumberFormatLocal = "@";//文本格式
                    range.ColumnWidth = 10; //"次数";

                    range = xlSheet.get_Range(xlSheet.Cells[3, 5], xlSheet.Cells[rowIndex + this.listView3.Items.Count, 5]); //
                    range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;// 设置单元格水平居左
                    range.NumberFormatLocal = "yyyy-MM-dd HH:mm";//日期格式
                    range.ColumnWidth = 20; //"读取时间"

                    //-----------------------设置单元格--------------------------------------------------------------------------------
                    //标题栏 
                    range = xlSheet.get_Range(xlSheet.Cells[2, 1], xlSheet.Cells[2, 5]);
                    range.Interior.ColorIndex = 45;//设置标题背景色为 浅橙色 
                    range.Font.Bold = true;//标题字体加粗 

                    foreach (ListViewItem objItem in this.listView3.Items)
                    {
                        xlSheet.Cells[rowIndex, 1] = "'" + objItem.SubItems[0].Text;
                        xlSheet.Cells[rowIndex, 2] = "'" + objItem.SubItems[1].Text;
                        xlSheet.Cells[rowIndex, 3] = "'" + objItem.SubItems[2].Text;
                        xlSheet.Cells[rowIndex, 4] = "'" + objItem.SubItems[3].Text;
                        xlSheet.Cells[rowIndex, 5] = "'" + objItem.SubItems[4].Text;

                        rowIndex += 1;
                    }

                    //数据区域
                    range = xlSheet.get_Range(xlSheet.Cells[2, 1], xlSheet.Cells[rowIndex, 5]);
                    range.Borders.LineStyle = 1;
                    range.Font.Size = 10;

                    range = xlSheet.get_Range(xlSheet.Cells[rowIndex, 1], xlSheet.Cells[rowIndex, 5]);
                    range.Merge(Missing.Value);         // 合并单元格 
                    // range.Borders[XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlThick;
                    // 设置单元格右边框加粗 
                    // range.Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlThick;
                    range.RowHeight = 30;
                    range.Value2 = "汇出时间: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;// 设置单元格水平居中   

                    //***** 格式设定 ****************************************************************************** 

                    if (xlSheet != null)
                    {
                        xlSheet.SaveAs(dialog.FileName, missing, missing, missing, missing, missing, missing, missing, missing, missing);
                        xlApp.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    string strMsg = ex.Message.ToString();
                    MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    xlApp.Quit();
                    throw;
                }
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            //开始读取
            if (0 == connect_OK)
            {
                nStop = false;
                MessageBox.Show("读写器未连接!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            button22.Enabled = true;//停止按钮
            button21.Enabled = false;//开始按钮

            button19.Enabled = false;//设置报警
            button20.Enabled = false;//检测报警

            nStop = true;
            teThreadEPC = new Thread(new ThreadStart(this.ThreadReaderEPC));
            teThreadEPC.Start();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            //停止读取
            button22.Enabled = false;//停止按钮
            button21.Enabled = true;//开始按钮

            button19.Enabled = true;//设置报警
            button20.Enabled = true;//检测报警

            nStop = false;
            Thread.Sleep(500);
            teThreadEPC.Abort();

            int nRow = listView3.Items.Count;
            if (nRow <= 0)
            {
                return;
            }

            comboBox6.Items.Clear();

            for (int i = 0; i < nRow; i++)
            {
                comboBox6.Items.Add(listView3.Items[i].SubItems[1].Text);
            }

            comboBox6.SelectedIndex = 0;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            //设置报警
            //EAS报警
            if (connect_OK == 0) return;

            string str;
            int i;
            if (comboBox6.SelectedIndex < 0)
            {
                MessageBox.Show("Please identify a tag first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (textBox22.Text.Length != 8)
            {
                MessageBox.Show("Please input correct accesspassword!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBox22.Focus();
                textBox22.SelectAll();
                return;
            }
            str = textBox22.Text;
            for (i = 0; i < 4; i++)
            {
                AccessPassWord[i] = (byte)Convert.ToInt16((str[i * 2].ToString() + str[i * 2 + 1].ToString()), 16);
            }

            str = comboBox6.SelectedText;
            EPC_Word = str.Length / 2;
            for (i = 0; i < EPC_Word; i++)
            {
                IDTemp[i] = Convert.ToByte(str.Substring(i * 2, 2), 16);
            }

            int Alarm = 0;
            if (radioButton13.Checked == true)
                Alarm = 0;
            if (radioButton14.Checked == true)
                Alarm = 1;


            for (i = 0; i < 5; i++)
            {
                Thread.Sleep(30);
                switch (ComMode)
                {
                    case 1:
                        res = Reader.EPC1G2_ChangeEas(m_hScanner, Convert.ToByte(EPC_Word), IDTemp, Convert.ToByte(Alarm), AccessPassWord, 0);
                        break;

                    case 3:
                        res = Reader.EPC1G2_ChangeEas(m_hScanner, Convert.ToByte(EPC_Word), IDTemp, Convert.ToByte(Alarm), AccessPassWord, RS485Address);
                        break;
                }
                if (res == _OK) break;
            }

            if (res == _OK)
            {
                MessageBox.Show("Set EAS alarm successfully!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Fail to set EAS alarm!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            //检测报警
            //检测EAS报警
            if (connect_OK == 0)
                return;

            int[] timrinterval = new int[] { 10, 20, 30, 50, 100, 200, 500 };
            Interval = timrinterval[4];

            if (button20.Text == "EasAlarm")
            {
                button20.Text = "Stop";
                Read_times = 0;
                timer1.Interval = Interval;
                timer1.Enabled = true;
            }
            else
            {
                button20.Text = "EasAlarm";
                timer1.Enabled = false;
            }
        }

        #region Timer
        private void timer1_Tick(object sender, EventArgs e)
        {
            byte[] DB = new byte[128];
            byte[] IDBuffer = new byte[7680];

            Read_times++;
            Thread.Sleep(Interval);

            ListViewItem item = new ListViewItem();

            //EAS报警
            switch (ComMode)
            {
                case 1:
                    res = Reader.EPC1G2_EasAlarm(m_hScanner, 0);
                    break;
                case 3:
                    res = Reader.EPC1G2_EasAlarm(m_hScanner, RS485Address);
                    break;
            }
            if (res == _OK)
            {
                panel2.BackColor = Color.Red;
            }
            else
            {
                panel2.BackColor = Color.Black;
            }
        }
        #endregion

        private void ThreadReaderEPC()
        {
            int t;
            int i = 0;
            int j, nCounter = 0, ID_len = 0;//ID_len_temp=0;
            string str, str_temp;
            byte[] temp = new byte[512];
            byte[] IDBuffer = new byte[30 * 256];
            byte[] DB = new byte[128];
            byte[] mask = new byte[512];
            byte[] IDTemp = new byte[512];
            int mem, ptr, len, EPC_Word;
            byte[] AccessPassword = new byte[4];

            bool iCheckTid = checkBox11.Checked;//TID
            UInt16 resTid = 2;

            bool iCheckUsr = checkBox12.Checked;//User
            UInt16 resUsr = 2;

            bool iCheckPas = checkBox13.Checked;//Password
            UInt16 resPas = 2;

            string stEpc = "";
            string stTid = "";
            string stUsr = "";
            string stPass = "";

            while (nStop)
            {
                resTid = 2;
                resUsr = 2;
                resPas = 2;

                if (connect_OK == 0)
                {
                    nStop = false;
                    MessageBox.Show("读写器未连接!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                }

                len = 0; ptr = 0; mem = 1; ID_len = 0; j = 0;   //nRow=0;
                for (i = 0; i < 512; i++) mask[i] = 0;

                for (i = 0; i < 3; i++)
                {
                    Thread.Sleep(300);
                    switch (ComMode)
                    {
                        case 0:
                            break;
                        case 1:
                        case 3://usb,读EPC号
                            res = Reader.EPC1G2_ReadLabelID(m_hScanner, mem, ptr, len, mask, IDBuffer, ref nCounter, RS485Address);
                            break;
                    }
                    if (_OK == res)
                    {
                        break;
                    }
                }

                if (res == _OK)
                {
                    string stroutput = string.Format("EPC1G2_ReadLabelID:OK({0,0:d})\r\n", nCounter);
                    Reader.VH_WriteAppLogFile("", 0, stroutput);

                    str = "";
                    ID_len = (int)IDBuffer[0];  //IDBuffer[0]为Word总数;1word=2Byte
                    for (j = 0; j < ID_len * 2; j++) temp[j] = IDBuffer[j + 1];

                    for (j = 0; j < ID_len * 2; j++)
                    {
                        str_temp = string.Format("{0,0:X02}", temp[j]);
                        str = str + str_temp;
                    }
                    stEpc = str;

                    //如果选中TID则读之
                    if (checkBox11.Checked)
                    {
                        for (j = 0; j < 4; j++) AccessPassword[j] = 0;  //memset(AccessPassword, 0, 4);
                        EPC_Word = ID_len;
                        for (j = 0; j < ID_len * 2; j++) IDTemp[j] = IDBuffer[j + 1]; //memcpy(IDTemp, &IDBuffer[1], ID_len * 2);
                        mem = 2;
                        str = textBox23.Text;
                        ptr = 0;
                        int.TryParse(str, out ptr);//字符串转数字

                        str = textBox24.Text;
                        len = 0;
                        int.TryParse(str, out len);//字符串转数字

                        for (j = 0; j < 3; j++)
                        {
                            switch (ComMode)
                            {
                                case 0:
                                    break;
                                case 1:
                                case 3://usb
                                    res = Reader.EPC1G2_ReadWordBlock(m_hScanner, (byte)EPC_Word, IDTemp, (byte)mem, (byte)ptr, (byte)len, DB, AccessPassword, RS485Address);
                                    break;
                            }
                            if (_OK == res)
                            {
                                resTid = (ushort)_OK;
                                break;
                            }
                        }
                        if (res == _OK)
                        {
                            str = "";
                            for (j = 0; j < 512; j++) temp[j] = 0;
                            for (j = 0; j < len * 2; j++) temp[j] = DB[j];
                            for (j = 0; j < len * 2; j++)
                            {
                                str_temp = string.Format("{0,0:X}", temp[j]);
                                str = str + str_temp;
                            }
                            stTid = str;
                        }
                    }

                    //如果选中USER则读之
                    if (checkBox13.Checked)
                    {
                        for (j = 0; j < 4; j++) AccessPassword[j] = 0;  //memset(AccessPassword, 0, 4);
                        EPC_Word = ID_len;
                        for (j = 0; j < ID_len * 2; j++) IDTemp[j] = IDBuffer[j + 1]; //memcpy(IDTemp, &IDBuffer[1], ID_len * 2);
                        mem = 3;

                        str = textBox27.Text;
                        ptr = 0;
                        int.TryParse(str, out ptr);//字符串转数字

                        str = textBox28.Text;
                        len = 0;
                        int.TryParse(str, out len);//字符串转数字

                        for (j = 0; j < 3; j++)
                        {
                            Thread.Sleep(100);
                            switch (ComMode)
                            {
                                case 0:
                                    break;
                                case 1:
                                case 3://usb
                                    res = Reader.EPC1G2_ReadWordBlock(m_hScanner, (byte)EPC_Word, IDTemp, (byte)mem, (byte)ptr, (byte)len, DB, AccessPassword, RS485Address);
                                    break;
                            }
                            if (_OK == res)
                            {
                                resTid = (ushort)_OK;
                                break;
                            }
                        }
                        if (res == _OK)
                        {
                            str = "";
                            for (j = 0; j < 512; j++) temp[j] = 0; //memset(temp, 0, 512);
                            for (j = 0; j < len * 2; j++) temp[j] = DB[j]; //memcpy(temp, DB, len * 2);
                            for (j = 0; j < len * 2; j++)
                            {
                                str_temp = string.Format("{0,0:X}", temp[j]);
                                str = str + str_temp;
                            }
                            stUsr = str;
                        }
                    }

                    //如果选中PASSWORD则读之
                    if (checkBox12.Checked)
                    {
                        for (j = 0; j < 4; j++) AccessPassword[j] = 0;  //memset(AccessPassword, 0, 4);
                        EPC_Word = ID_len;
                        for (j = 0; j < ID_len * 2; j++) IDTemp[j] = IDBuffer[j + 1]; //memcpy(IDTemp, &IDBuffer[1], ID_len * 2);
                        mem = 0;

                        str = textBox25.Text;
                        ptr = 0;
                        int.TryParse(str, out ptr);//字符串转数字

                        str = textBox26.Text;
                        len = 0;
                        int.TryParse(str, out len);//字符串转数字

                        for (j = 0; j < 3; j++)
                        {
                            Thread.Sleep(100);
                            switch (ComMode)
                            {
                                case 0:
                                    break;
                                case 1:
                                case 3://usb
                                    res = Reader.EPC1G2_ReadWordBlock(m_hScanner, (byte)EPC_Word, IDTemp, (byte)mem, (byte)ptr, (byte)len, DB, AccessPassword, RS485Address);
                                    break;
                            }
                            if (_OK == res)
                            {
                                resTid = (ushort)_OK;
                                break;
                            }
                        }
                        if (res == _OK)
                        {
                            str = "";
                            for (j = 0; j < 512; j++) temp[j] = 0; //memset(temp, 0, 512);
                            for (j = 0; j < len * 2; j++) temp[j] = DB[j]; //memcpy(temp, DB, len * 2);
                            for (j = 0; j < len * 2; j++)
                            {
                                str_temp = string.Format("{0,0:X}", temp[j]);
                                str = str + str_temp;
                            }
                            stPass = str;
                        }
                    }

                    string strTemp;
                    string tmp;
                    int nS;

                    nS = listView3.Items.Count;
                    for (i = 0; i < nS; i++)
                    {
                        strTemp = listView3.Items[i].SubItems[1].Text;
                        if (stEpc == strTemp)
                        {
                            strTemp = "";
                            break;
                        }
                    }
                    if (i == nS)
                    {
                        //新增
                        ListViewItem item = new ListViewItem();
                        item = listView3.Items.Add((nS + 1).ToString(), (nS + 1).ToString());
                        item.SubItems.Add(stEpc);
                        item.SubItems.Add("");
                        item.SubItems.Add("");
                        item.SubItems.Add("");

                    }
                    if (checkBox13.Checked)
                    {
                        if (stUsr.Length != 0)
                        {
                            listView3.Items[i].SubItems[2].Text = stUsr;
                            stUsr = "";
                        }
                    }
                    if (checkBox11.Checked)
                    {
                        if (stTid.Length != 0)
                        {
                            //不为空给值
                            listView3.Items[i].SubItems[3].Text = stTid;
                            stTid = "";
                        }
                    }
                    if (checkBox12.Checked)
                    {
                        if (stPass.Length != 0)
                        {
                            listView3.Items[i].SubItems[4].Text = stPass;
                            stPass = "";
                        }
                    }
                    tmp = stEpc;
                    label46.Text = tmp;

                    int ichFlag = 0;
                    if (iCheckTid && iCheckUsr && iCheckPas) //全选
                    {
                        if (resTid == _OK && resPas == _OK && resUsr == _OK)
                        {
                            ichFlag = 1;
                        }
                    }
                    else if (iCheckTid && iCheckUsr)
                    {
                        if (resTid == _OK && resUsr == _OK)
                        {
                            ichFlag = 1;
                        }
                    }
                    else if (iCheckTid && iCheckPas)
                    {
                        if (resTid == _OK && resPas == _OK)
                        {
                            ichFlag = 1;
                        }
                    }
                    else if (iCheckUsr && iCheckPas)
                    {
                        if (resUsr == _OK && resPas == _OK)
                        {
                            ichFlag = 1;
                        }
                    }
                    else if (iCheckTid)
                    {
                        if (resTid == _OK)
                        {
                            ichFlag = 1;
                        }
                    }
                    else if (iCheckUsr)
                    {
                        if (resUsr == _OK)
                        {
                            ichFlag = 1;
                        }
                    }
                    else if (iCheckPas)
                    {
                        if (resPas == _OK)
                        {
                            ichFlag = 1;
                        }
                    }
                    else
                    {
                        ichFlag = 1;
                    }

                    if (1 == ichFlag)
                    {
                        //开始报警
                        if (3 == ComMode)
                        {
                            Reader.BuzzerAlarm(m_hScanner, 1);
                        }
                        Thread.Sleep(100);
                        //停止报警
                        if (3 == ComMode)
                        {
                            Reader.BuzzerAlarm(m_hScanner, 0);
                        }
                    }
                }
                else
                {
                    j++;
                    t = res * 1000 + j;
                    panel1.BackColor = Color.Yellow;
                    label44.Text = string.Format("等待读卡，请放入卡:{0,0:d}", t);
                    Thread.Sleep(300);
                }

                str = textBox29.Text;
                j = 0;
                int.TryParse(str, out j);//字符串转数字
                if ((j <= 0) || (j > 60))
                {
                    j = 1;
                }
                for (i = 0; i < (int)j; i++)
                {
                    label44.Text = string.Format("等待读卡，请放入卡:{0,0:d}{1,0:s}{2,0:d}", i / 1000, "-", i % 1000);
                    Thread.Sleep(100);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel files(*.xls)|*.xls";
            string path;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                path = ofd.FileName;
                //循环添加
                DataTable dt = LoadDataFromExcel(path).Tables[0];
                foreach (DataRow row in dt.Rows)
                {
                    ListViewItem item = new ListViewItem();

                    item = listView1.Items.Add(row[0].ToString());
                    item.SubItems.Add(row[1].ToString());
                    item.SubItems.Add(row[2].ToString());
                    item.SubItems.Add(row[3].ToString());
                }
            }
        }

        public static DataSet LoadDataFromExcel(string filePath)
        {
            try
            {
                string strConn;
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=False;IMEX=1'";
                OleDbConnection OleConn = new OleDbConnection(strConn);
                OleConn.Open();
                String sql = "SELECT * FROM  [Sheet1$]";
                OleDbDataAdapter OleDaExcel = new OleDbDataAdapter(sql, OleConn);
                DataSet OleDsExcle = new DataSet();
                OleDaExcel.Fill(OleDsExcle, "Sheet1");
                OleConn.Close();
                return OleDsExcle;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据绑定Excel失败!失败原因：" + e.Message, "提示信息",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
        }

        List<string> listaddcon = new List<string>();

        public void TimerEpcLoad()
        {
            int i = 0;
            int j, nCounter = 0, ID_len = 0;
            string str, str_temp;
            byte[] temp = new byte[512];
            byte[] IDBuffer = new byte[30 * 256];
            byte[] DB = new byte[128];
            byte[] mask = new byte[512];
            byte[] IDTemp = new byte[512];
            int mem, ptr, len, EPC_Word;
            byte[] AccessPassword = new byte[4];
            int counts = 0;

            string stData = "";

            while (nStop)
            {
                if (connect_OK == 0)
                {
                    nStop = false;
                    MessageBox.Show("读写器未连接!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                }

                len = 0; ptr = 0; mem = 1; ID_len = 0; j = 0;

                for (i = 0; i < 512; i++) mask[i] = 0;

                for (i = 0; i < 3; i++)
                {
                    switch (ComMode)
                    {
                        case 0:
                            break;
                        case 1:
                        case 3://usb,读EPC号
                            res = Reader.EPC1G2_ReadLabelID(m_hScanner, mem, ptr, len, mask, IDBuffer, ref nCounter, RS485Address);
                            counts++;
                            break;
                    }
                    if (_OK == res)
                    {
                        break;
                    }
                }

                if (res == _OK)
                {
                    str = "";
                    ID_len = (int)IDBuffer[0];  //IDBuffer[0]为Word总数;1word=2Byte
                    for (j = 0; j < ID_len * 2; j++) temp[j] = IDBuffer[j + 1];

                    for (j = 0; j < ID_len * 2; j++)
                    {
                        str_temp = string.Format("{0,0:X02}", temp[j]);
                        str = str + str_temp;
                    }
                    stData = str;
                    if (!listaddcon.Contains(str))
                    {
                        comboBox7.Items.Add(str);
                        comboBox7.SelectedIndex = 0;
                        comboBox8.Items.Add(str);
                        comboBox8.SelectedIndex = 0;
                        comboBox10.Items.Add(str);
                        comboBox10.SelectedIndex = 0;
                        listaddcon.Add(str);
                    }

                    //如果选中TID则读之
                    if (radioButton15.Checked)
                    {
                        for (j = 0; j < 4; j++) AccessPassword[j] = 0;  //memset(AccessPassword, 0, 4);
                        EPC_Word = ID_len;
                        for (j = 0; j < ID_len * 2; j++) IDTemp[j] = IDBuffer[j + 1]; //memcpy(IDTemp, &IDBuffer[1], ID_len * 2);
                        mem = 2;
                        str = textBox23.Text;
                        ptr = 0;
                        int.TryParse(str, out ptr);//字符串转数字

                        str = textBox24.Text;
                        len = 0;
                        int.TryParse(str, out len);//字符串转数字

                        for (j = 0; j < 3; j++)
                        {
                            switch (ComMode)
                            {
                                case 0:
                                    break;
                                case 1:
                                case 3://usb
                                    res = Reader.EPC1G2_ReadWordBlock(m_hScanner, (byte)EPC_Word, IDTemp, (byte)mem, (byte)ptr, (byte)len, DB, AccessPassword, RS485Address);
                                    break;
                            }
                            if (_OK == res)
                            {
                                break;
                            }
                        }
                        if (res == _OK)
                        {
                            str = "";
                            for (j = 0; j < 512; j++) temp[j] = 0;
                            for (j = 0; j < len * 2; j++) temp[j] = DB[j];
                            for (j = 0; j < len * 2; j++)
                            {
                                str_temp = string.Format("{0,0:X}", temp[j]);
                                str = str + str_temp;
                            }
                            stData = str;
                        }
                    }

                    //如果选中USER则读之
                    if (radioButton18.Checked)
                    {
                        for (j = 0; j < 4; j++) AccessPassword[j] = 0;
                        EPC_Word = ID_len;
                        for (j = 0; j < ID_len * 2; j++) IDTemp[j] = IDBuffer[j + 1];
                        mem = 3;

                        str = textBox27.Text;
                        ptr = 0;
                        int.TryParse(str, out ptr);//字符串转数字

                        str = textBox28.Text;
                        len = 0;
                        int.TryParse(str, out len);//字符串转数字

                        for (j = 0; j < 3; j++)
                        {
                            Thread.Sleep(300);
                            switch (ComMode)
                            {
                                case 0:
                                    break;
                                case 1:
                                case 3://usb
                                    res = Reader.EPC1G2_ReadWordBlock(m_hScanner, (byte)EPC_Word, IDTemp, (byte)mem, (byte)ptr, (byte)len, DB, AccessPassword, RS485Address);
                                    break;
                            }
                            if (_OK == res)
                            {
                                break;
                            }
                        }
                        if (res == _OK)
                        {
                            str = "";
                            for (j = 0; j < 512; j++) temp[j] = 0; //memset(temp, 0, 512);
                            for (j = 0; j < len * 2; j++) temp[j] = DB[j]; //memcpy(temp, DB, len * 2);
                            for (j = 0; j < len * 2; j++)
                            {
                                str_temp = string.Format("{0,0:X}", temp[j]);
                                str = str + str_temp;
                            }
                            stData = str;
                        }
                    }

                    //如果选中PASSWORD则读之
                    if (radioButton17.Checked)
                    {
                        for (j = 0; j < 4; j++) AccessPassword[j] = 0;  //memset(AccessPassword, 0, 4);
                        EPC_Word = ID_len;
                        for (j = 0; j < ID_len * 2; j++) IDTemp[j] = IDBuffer[j + 1]; //memcpy(IDTemp, &IDBuffer[1], ID_len * 2);
                        mem = 0;

                        str = textBox25.Text;
                        ptr = 0;
                        int.TryParse(str, out ptr);//字符串转数字

                        str = textBox26.Text;
                        len = 0;
                        int.TryParse(str, out len);//字符串转数字

                        for (j = 0; j < 3; j++)
                        {
                            Thread.Sleep(100);
                            switch (ComMode)
                            {
                                case 0:
                                    break;
                                case 1:
                                case 3://usb
                                    res = Reader.EPC1G2_ReadWordBlock(m_hScanner, (byte)EPC_Word, IDTemp, (byte)mem, (byte)ptr, (byte)len, DB, AccessPassword, RS485Address);
                                    break;
                            }
                            if (_OK == res)
                            {
                                break;
                            }
                        }
                        if (res == _OK)
                        {
                            str = "";
                            for (j = 0; j < 512; j++) temp[j] = 0; //memset(temp, 0, 512);
                            for (j = 0; j < len * 2; j++) temp[j] = DB[j]; //memcpy(temp, DB, len * 2);
                            for (j = 0; j < len * 2; j++)
                            {
                                str_temp = string.Format("{0,0:X}", temp[j]);
                                str = str + str_temp;
                            }
                            stData = str;
                        }
                    }

                    string strTemp;
                    string tmp;
                    int nS;

                    nS = listView4.Items.Count;
                    for (i = 0; i < nS; i++)
                    {
                        strTemp = listView4.Items[i].SubItems[1].Text;

                        if (stData == strTemp)
                        {
                            int times = Convert.ToInt32(listView4.Items[i].SubItems[3].Text);
                            times++;
                            listView4.Items[i].SubItems[3].Text = times.ToString();
                            strTemp = "";
                            break;
                        }
                    }
                    if (i == nS)
                    {
                        //新增
                        ListViewItem item = new ListViewItem();
                        item = listView4.Items.Add((nS + 1).ToString(), (nS + 1).ToString());
                        item.SubItems.Add(stData);
                        item.SubItems.Add((stData.Length / 4).ToString());
                        item.SubItems.Add("1");
                        item.SubItems.Add(counts.ToString());

                    }
                    if (checkBox13.Checked)
                    {
                        if (stData.Length != 0)
                        {
                            listView4.Items[i].SubItems[1].Text = stData;
                            listView4.Items[i].SubItems[2].Text = (stData.Length / 4).ToString();
                            stData = "";
                        }
                    }
                    if (checkBox11.Checked)
                    {
                        if (stData.Length != 0)
                        {
                            //不为空给值
                            listView4.Items[i].SubItems[1].Text = stData;
                            listView4.Items[i].SubItems[2].Text = (stData.Length / 4).ToString();
                            stData = "";
                        }
                    }
                    if (checkBox12.Checked)
                    {
                        if (stData.Length != 0)
                        {
                            listView4.Items[i].SubItems[1].Text = stData;
                            listView4.Items[i].SubItems[2].Text = (stData.Length / 4).ToString();
                            stData = "";
                        }
                    }
                    tmp = stData;

                    for (i = 0; i < nS; i++)
                    {
                        listView4.Items[i].SubItems[4].Text = counts.ToString();
                    }
                }
            }
        }
        #endregion

        private void button6_Click(object sender, EventArgs e)
        {
            if (connect_OK == 0)
            {
                return;
            }

            if (button6.Text == "List Tag ID")
            {
                listaddcon.Clear();
                listView4.Items.Clear();
                comboBox7.Items.Clear();
                comboBox8.Items.Clear();
                comboBox10.Items.Clear();
                nStop = true;
                button6.Text = "Stop";
                TimerEPC = new Thread(new ThreadStart(this.TimerEpcLoad));
                TimerEPC.Start();
            }
            else
            {
                TimerEPC.Abort();
                button6.Text = "List Tag ID";
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            if (connect_OK == 0)
            {
                return;
            }

            int i;
            string str;

            str = textBox35.Text;
            for (i = 0; i < 4; i++)
            {
                AccessPassWord[i] = Convert.ToByte(str.Substring(i * 2, 2), 16);
            }

            EPC_Word = comboBox8.Text.Length / 4;

            byte[] stb = strTobyte(comboBox8.Text);

            for (i = 0; i < EPC_Word * 2; i++)
            {
                IDTemp[i] = stb[i];
            }
            if (radioButton22.Checked == true)
                mem = 0;
            if (radioButton21.Checked == true)
                mem = 1;
            if (radioButton20.Checked == true)
                mem = 2;
            if (radioButton19.Checked == true)
                mem = 3;

            if (button26.Text == "Read")
            {
                button26.Text = "Stop";
                Read_times = 0;
                listBox1.Items.Clear();
                int[] timrinterval = new int[] { 10, 20, 30, 50, 100, 200, 500 };
                timer2.Interval = timrinterval[comboBox9.SelectedIndex];
                timer2.Enabled = true;
            }
            else
            {
                button26.Text = "Read";
                timer2.Enabled = false;
            }
        }


        public byte[] strTobyte(string str)
        {
            byte[] bytes = new byte[20];
            int j = 0;

            for (int i = 0; i < str.Length / 2; i++)
            {
                bytes[i] = Convert.ToByte(str.Substring(j, 2), 16);
                j++;
                j++;
            }

            return bytes;

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            string str, strtemp;
            byte[] DB = new byte[128];

            res = Reader.EPC1G2_ReadWordBlock(m_hScanner, Convert.ToByte(EPC_Word), IDTemp, Convert.ToByte(mem), Convert.ToByte(textBox37.Text), Convert.ToByte(textBox36.Text), DB, AccessPassWord, RS485Address);

            if (res == _OK)
            {
                str = "";
                for (int i = 0; i < Convert.ToByte(textBox36.Text) * 2; i++)
                {
                    strtemp = DB[i].ToString("X2");
                    str += strtemp;
                }
                listBox1.Items.Add(str);
            }
            else
            {
                listBox1.Items.Add("No Data");
            }

        }

        private void button25_Click(object sender, EventArgs e)
        {
            if (connect_OK == 0)
            {
                return;
            }

            int i;
            string str;
            byte[] mask = new byte[96];
            listBox1.Items.Clear();

            str = textBox35.Text;
            for (i = 0; i < 4; i++)
            {
                AccessPassWord[i] = (byte)Convert.ToInt16((str[i * 2].ToString() + str[i * 2 + 1].ToString()), 16);
            }

            if (radioButton22.Checked == true)
                mem = 0;
            if (radioButton21.Checked == true)
                mem = 1;
            if (radioButton20.Checked == true)
                mem = 2;
            if (radioButton19.Checked == true)
                mem = 3;

            str = textBox34.Text;
            for (i = 0; i < textBox34.Text.Length / 2; i++)
            {
                mask[i] = (byte)Convert.ToInt16((str[i * 2].ToString() + str[i * 2 + 1].ToString()), 16);
            }

            len = Convert.ToInt16(textBox36.Text);
            if (mem == 1)
            {
                for (i = 0; i < 5; i++)
                {
                    res = Reader.EPC1G2_WriteEPC(m_hScanner, Convert.ToByte(textBox36.Text), mask, AccessPassWord, RS485Address);

                    if ((res == _OK) || (res == 5) || (res == 8) || (res == 9))
                        break;
                }
            }
            else
            {
                if (comboBox8.SelectedIndex < 0)
                {
                    MessageBox.Show("Please read first than choose a tag!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                switch (mem)
                {
                    case 0:
                    case 2:
                        if (Convert.ToInt16(textBox37.Text) < 0 || Convert.ToInt16(textBox37.Text) > 3)
                        {
                            MessageBox.Show("Please input Location of Tag Address between 0 and 4!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            textBox37.Focus();
                            textBox37.SelectAll();
                            return;
                        }
                        if ((len + Convert.ToInt16(textBox37.Text)) > 4)
                        {
                            len = 4 - Convert.ToInt16(textBox37.Text);
                        }
                        break;
                    case 3:
                        if (Convert.ToInt16(textBox37.Text) < 0)
                        {
                            MessageBox.Show("Please input Location of Tag Address more than 0!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            textBox37.Focus();
                            textBox37.SelectAll();
                            return;
                        }
                        break;
                }
                EPC_Word = comboBox8.Text.Length / 4;

                byte[] stb = strTobyte(comboBox8.Text);

                for (i = 0; i < EPC_Word * 2; i++)
                {
                    IDTemp[i] = stb[i];
                }

                for (i = 0; i < 5; i++)
                {
                    res = Reader.EPC1G2_WriteWordBlock(m_hScanner, Convert.ToByte(EPC_Word), IDTemp, Convert.ToByte(mem), Convert.ToByte(textBox37.Text), Convert.ToByte(len), mask, AccessPassWord, RS485Address);

                    if ((res == _OK) || (res == 5) || (res == 8) || (res == 9))
                        break;
                }
            }

            if (res == _OK)
            {
                listBox1.Items.Add("Write Successfully!");
            }
            else
            {
                listBox1.Items.Add("Write Fail!");
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            // kill Tag
            if (connect_OK == 0)
                return;
            int i;
            string str, strtemp;
            byte[] KillPassWord = new byte[4];


            str = textBox33.Text;
            for (i = 0; i < 4; i++)
            {
                strtemp = str[i * 2].ToString() + str[i * 2 + 1].ToString();
                KillPassWord[i] = (byte)Convert.ToInt16(strtemp, 16);
            }

            EPC_Word = comboBox7.Text.Length / 4;

            byte[] stb = strTobyte(comboBox7.Text);

            for (i = 0; i < EPC_Word * 2; i++)
            {
                IDTemp[i] = stb[i];
            }

            if (MessageBox.Show("Are you sure to kill the tag ?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            for (i = 0; i < 3; i++)
            {
                res = Reader.EPC1G2_KillTag(m_hScanner, Convert.ToByte(EPC_Word), IDTemp, KillPassWord, RS485Address);
                if ((res == _OK) || (res == 5) || (res == 8) || (res == 9))
                {
                    break;
                }
            }
            if (res == _OK)
            {
                MessageBox.Show("kill tag successfully!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show("Kill tag Fail!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            if (connect_OK == 0) return;

            string str;
            int i;
            byte locked = 0;

            str = textBox38.Text;
            for (i = 0; i < 4; i++)
            {
                AccessPassWord[i] = (byte)Convert.ToInt16((str[i * 2].ToString() + str[i * 2 + 1].ToString()), 16);
            }

            EPC_Word = comboBox10.Text.Length / 4;

            byte[] stb = strTobyte(comboBox10.Text);

            for (i = 0; i < EPC_Word * 2; i++)
            {
                IDTemp[i] = stb[i];
            }

            if (MessageBox.Show("Are you sure to protect this tag?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            if (radioButton36.Checked == true)
            {
                if (radioButton32.Checked == true)
                    mem = 0;
                if (radioButton31.Checked == true)
                    mem = 1;
                if (radioButton30.Checked == true)
                    locked = 4;
                if (radioButton29.Checked == true)
                    locked = 5;
                if (radioButton28.Checked == true)
                    locked = 6;
                if (radioButton27.Checked == true)
                    locked = 7;
            }
            else
            {
                if (radioButton35.Checked == true)
                    mem = 2;
                if (radioButton34.Checked == true)
                    mem = 3;
                if (radioButton33.Checked == true)
                    mem = 4;

                if (radioButton26.Checked == true)
                    locked = 0;
                if (radioButton25.Checked == true)
                    locked = 1;
                if (radioButton24.Checked == true)
                    locked = 2;
                if (radioButton23.Checked == true)
                    locked = 3;
            }

            for (i = 0; i < 5; i++)
            {
                res = Reader.EPC1G2_SetLock(m_hScanner, Convert.ToByte(EPC_Word), IDTemp, Convert.ToByte(mem), locked, AccessPassWord, RS485Address);

                if ((res == _OK) || (res == 5) || (res == 8) || (res == 9))
                    break;
            }

            if ((res == _OK) || (res == 5) || (res == 8) || (res == 9))
            {
                MessageBox.Show("Set protect successfully!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Fail to set protect!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void radioButton36_CheckedChanged(object sender, EventArgs e)
        {
            groupBox34.Enabled = true;
        }

        private void radioButton35_CheckedChanged(object sender, EventArgs e)
        {
            groupBox34.Enabled = false;
        }

        private void radioButton34_CheckedChanged(object sender, EventArgs e)
        {
            groupBox34.Enabled = false;
        }

        private void radioButton33_CheckedChanged(object sender, EventArgs e)
        {
            groupBox34.Enabled = false;
        }
    }
}
