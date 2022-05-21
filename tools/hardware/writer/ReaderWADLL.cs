using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Data;

namespace CsharpDemoWC
{
    public class Reader
    {
        /// <summary>
        /// 定义宏
        /// 定义一个动态链接库的文件名
        /// </summary>
        public const string PUBLIC_COM_PATH = @"VHLib.dll";//"ReaderDynamicLib.dll";//@"VHLib.dll";

        //==============================常量定义==============================
        public const int ID_MAX_SIZE_64BIT	=   8;		//电子标签的ID号为64bit
        public const int ID_ATA_MAX_SIZE	=   20;		//ATA电子标签的ID号为20byte
        public const int ID_MAX_SIZE_96BIT	=   65;		//电子标签的ID号
        public const int MAX_LABELS			=   255;		// 一次读写操作最多不超过100个标签

        //==============================API函数返回码==============================
        public const int _OK				=   0x00;	// 操作成功

        //通信方面出错信息
        public const int _init_rs232_err	=   0x81;	//  通信口初始化失败
        public const int _no_scanner		=   0x82;	//  找不到读写器
        public const int _comm_error        =   0x83;	//  收发通信数据出错
        public const int _baudrate_error    =   0x84;	//  设置波特率出错
        public const int _init_net_error	=   0x85;	//  网口初始化失败
        public const int _net_error         =   0x86;	//  网口收发通信数据出错

        // 读写器返回出错信息
        public const int _no_antenna		=   0x01;   //天线连接失败
        public const int _no_label			=   0x02;   //未检测到标签
        public const int _invalid_label		=   0x03;   //非法标签
        public const int _less_power		=   0x04;   //读写功率不够
        public const int _write_prot_error	=   0x05;   //该区写保护
        public const int _check_sum_error	=   0x06;   //校验和错误
        public const int _parameter_error	=   0x07;   //参数错误
        public const int _memory_error		=   0x08;   //数据区不存在
        public const int _password_error	=	0x09;   //密码不正确
        public const int _killpassword_error=	0x0a;   //G2标签毁灭密码为全零
        public const int _nonlicet_command	=   0x0b;   //非法命令
        public const int _nonlicet_user		=   0x0c;   //密码不匹配的非法用户
        public const int _invalid_command	=   0x1e;   //表示无效指令，如参数不正确的指令
        public const int _other_error       =   0x1f;   //未知命令

        //函数输入错误
        public const int _no_cardID_input   =    0x20;   //其它错误

        public struct ReaderDate
        {
            public byte Year;			//年
            public byte Month;			//月
            public byte Day;			//日
            public byte Hour;			//时
            public byte Minute;			//分
            public byte Second;			//秒
        }

        public struct HandsetParam
        {
            public byte TagType;		//标签种类：01H－ISO18000-6B，02H－EPCC1，04H－EPCC1G2，08H－EM4442。	(0)
            public byte Alarm;			//bit0-bit7 bit0:0-不报警，1-报警 bit1 0-不使用白名单 1-使用白名单。	(1)
            public byte OutputMode;	    //数据输出模式：0-保存并直接输出,1-保存但不直接输出,2-不保存但直接输出  (2)
            public byte USBBaudRate;	//USB接口波特率 04-9600 05-19200  06-38400  07-57600 08-115200			(3)
            public byte BtBaudRate;		//蓝牙串口通讯波特率													(4)
            public byte Min_Frequence;	//发射微波信号频率的起始点，取值： 1~63。								(5)
            public byte Max_Frequence;	//发射微波信号频率的起始点，取值： 1~63。								(6)
            public byte Power;			//发射功率值，取值：0~160。												(7)
            public byte RFhrdVer1;		//RF模块硬件主版本														(8)
            public byte RFhrdVer2;		//RF模块硬件次版本														(9)
            public byte RFSoftVer1;		//RF模块软件主版本														(10)
            public byte RFSoftVer2;		//RF模块软件次版本														(11)
            public byte ISTID;		    //是否读TID区															(12)
            public byte TIDAddr;		//TID读取起始位置														(13)
            public byte TIDLen;		    //TID读取长度															(14)
            public byte ISUSER;		    //是否读USER															(15)
            public byte USERAddr;		//USER读取起始位置														(16)
            public byte USERLen;		//USER读取长度															(17)
            public byte ISVibrate;		//电机振动,0-不，1-振动													(18)
            public byte Modutype;		//模块类型,0---R2000, 1---VM5E											(19)
            public byte Reserve21;		//保留																	(20)
            public byte Reserve22;		//保留																	(21)
            public byte Reserve23;		//保留																	(22)
            public byte Reserve24;		//保留																	(23)
            public byte Reserve25;		//保留																	(24)
            public byte Reserve26;		//保留																	(25)
            public byte Reserve27;		//保留																	(26)
            public byte Reserve28;		//保留																	(27)
            public byte Reserve29;		//保留																	(28)
            public byte Reserve30;		//保留																	(29)
            public byte Reserve31;		//保留																	(30)
            public byte Reserve32;		//保留
        }

        public struct ReaderBasicParam
        {
            public byte BaudRate;			//串口的通信速率，取值：00H~08H，代表速率同"设定波特率"命令。
            public byte Power;				//发射功率值，取值：20~30dBm。0-63
            public byte Min_Frequence;		//发射微波信号频率的起始点，取值： 0~59。
            public byte Max_Frequence;		//发射微波信号频率的终止点，取值： 0~59。
            public byte Reserve5;			//保留
            public byte WorkMode;			//读写器工作方式：0-主动方式，1-命令方式
            public byte ReaderAddress;		//RS485地址:1--254
            public byte NumofCard;			//最多读卡数目。
            public byte TagType;			//标签种类：01H－ISO18000-6B，02H－EPCC1，04H－EPCC1G2，08H－EM4442。
            public byte ReadDuration;		//(10)读卡持续时间：射频发射持续时间，只针对EM标签有效；0－10ms，1－20ms，2－30ms，3－40ms。
            public byte ReadTimes;			//读卡次数M：收到上位机读卡命令，读写器执行M次此命令。
            public byte EnableBuzzer;		//1:使能蜂鸣器0:不使能蜂鸣器
            public byte IP1;			    //读写器IP地址
            public byte IP2;			    //
            public byte IP3;			    //
            public byte IP4;			    //
            public byte Port1;				//读写器端口高位
            public byte Port2;				//
            public byte Mask1;				//读写器掩码1
            public byte Mask2;				//读写器掩码2
            public byte Mask3;				//读写器掩码3
            public byte Mask4;				//读写器掩码4
            public byte Gateway1;			//读写器地址网关
            public byte Gateway2;			//
            public byte Gateway3;			//
            public byte Gateway4;			//
            public byte MAC1;			    //读写器MAC地址
            public byte MAC2;			    //
            public byte MAC3;			    //
            public byte MAC4;			    //
            public byte MAC5;			    //
            public byte MAC6;			    //
        }

        //读写器主动工作参数
        public struct ReaderAutoParam
        {
            public byte AutoMode;			//(1)读标签模式：0-定时方式，1-触发方式。
            public byte TimeH;				//(2)标签保留时间：单位：秒s。缺省值为1。
            public byte TimeL;				//(3)
            public byte Interval;			//(4)0-10ms，1-20ms，2-30ms，3-50ms，4-100ms。缺省值为2。每隔设定时间主动读取一次标签。
            public byte NumH;				//(5)标签保留数目：缺省值为1。已读取的标签ID在读写器内存中保留的数目。
            public byte NumL;				//(6)
            public byte OutputManner;	    //(7)数据输出格式：0-简化格式，1-标准格式，2-XML格式。缺省值为0。
            public byte OutInterface;		//(8)输出接口：0－RS232口，1－RS485口，2－RJ45口。缺省值为0。 3- Wiegand26    4- Wiegand34
            public byte WiegandWidth;		//(9)Weigand脉冲宽度值。
            public byte WiegandInterval;	//(10)Weigand脉冲间隔值。
            public byte ID_Start;			//(11)输出卡号的起始位，取值0～4。
            public byte IDPosition;			//(12)卡号在电子标签上的存放地址。EPC区--0, USER区--1，TID区--2
            public byte Report_Interval;	//(13)通知间隔：单位秒s。缺省值为1。每隔设定时间主动通知上位机一次。
            public byte Report_Condition;	//(14)通知条件：缺省值为1。0-被动通知，1-定时通知，2-增加新标签，3-减少标签，4-标签数变化	
            public byte Report_Output;		//(15)通知输出端
            public byte Antenna;			//(16)天线选择。1-ant1,2-ant2,4-ant4,8-ant8
            public byte TriggerMode;	    //(17)触发方式(缺省值为0): 0-低电平 1-高电平
            public byte HostIP1;			//(18)被通知IP地址
            public byte HostIP2;			//(19)
            public byte HostIP3;			//(20)
            public byte HostIP4;			//(21)
            public byte Port1;				//(22)被通知端口
            public byte Port2;				//(23)
            public byte Reserve24;			//(24)被通知MAC,mofi by mqs 20121207 保留
            public byte ArgentinaSim;		//(25)//仿真模式(阿根廷),0--非仿真，1--仿真
            public byte CardTime1;			//(26)//读卡总超时1
            public byte CardTime2;			//(27)//读卡总超时2
            public byte ArgentinaMode;		//(28)//阿根的五种模式,0---Only ATA  只读ATA;1---Only EPC 只读EPC;2---Only EPC & TID 只读EPC+TID;3---ATA + EPC   读ATA+EPC;4---ATA + EPC & TID读ATA+EPC&TID
            public byte AnSleepTime;		//(29)//天线睡眠时间，单位：ms,(1-255)
            public byte Alarm;				//(30)0-不报警，1-报警。在定时和触发方式下是否检测报警。
            public byte Reserve31;		    //(31)标准输出时间间隔，缺省值为120秒，1～255。
            public byte EnableRelay;		//(32)自动工作模式是否控制继电器1:控制 0:不控制
        }

        public static int PERFIXLEN = 20;  //前缀值的长度
        //读写器仿真键盘参数
        //对应的C#结构体
        [StructLayout( LayoutKind.Sequential,Pack=1)]
        //[StructLayout(LayoutKind.Sequential)]
        public struct SimParam
        {
            public byte DataFormat;				//(0)数据格式两种：16进制--0, 10进制--1									
            //public byte	DataBank;				//(1)EPC区--0, USER区--1，TID区--2
            public byte IsPerfix;				//(2)是否前缀，1--带,0--不带												
            //public byte []PerfixCode;	//(3-22)前缀值,不够0x00填充													
            //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] PerfixCode;
            public byte IsEnter;				//(23)是否回车换行, 1--带,0--不带											
            public byte StartAddress;			//(24)数据起始地址															
            public byte DataLen;				//(25)数据长度
            public byte OutputInterval;	//(26)标准输出间隔(0-255)(0--输出ID之间无间隔)
            //public byte Demo1;
            //public byte Demo2;
            //public SimParam()
            //{
            //    PerfixCode = new byte[20];
            //}
        }

        public struct tagReaderFreq
        {
            public string chFreq;		//国家频率字符串

            public int iGrade;			//级数 = 50;
            public int iSkip;			//步进 = 500KHz;
            public int dwFreq;			//起始频率 = 902750;
            //公式：902750 + 级数*步进
        }

        //关于各个国家的频率
//static const tagReaderFreq stuctFreqCountry[]=
//{
//	{"00---FCC(American)", 63, 400, 902600},							//(0),{"00---FCC(American)", 50, 500, 902750},
//	{"01---ETSI EN 300-220(Europe300-220)", 11, 200, 865500},			//(1),{"01---ETSI EN 300-220(Europe300-220)", -1, -1, -1},
//	{"02---ETSI EN 302-208(Europe302-208)", 4, 600, 865700},			//(2)
//	{"03---HK920-925(Hong Kong)", 10, 500, 920250},						//(3)
//	{"04---TaiWan 922-928(Taiwan)", 12, 500, 922250},					//(4)
//	{"05---Japan 952-954(Japan)", 0, 0, 0},								//(5)
//	{"06---Japan 952-955(Japan)", 14,200, 952200},						//(6)
//	{"07---ETSI EN 302-208(Europe)", 4, 600, 865700},					//(7)
//	{"08---Korea 917-921(Korea)", 6, 600, 917300},						//(8)
//	{"09---Malaysia 919-923(Malaysia)", 8, 500, 919250},				//(9)
//	{"10--China 920-925(China)", 16, 250, 920625},						//(10)
//	{"11--Japan 952-956(Japan)", 4, 1200, 952400},						//(11)
//	{"12--South Africa 915-919(Poncho)", 17, 200, 915600},				//(12)
//	{"13--Brazil 902-907/915-928(Brazil)", 35, 500, 902750},			//(13)
//	{"14--Thailand 920-925(Thailand)", -1, -1, -1},						//(14)
//	{"15--Singapore 920-925(Singapore)", 10, 500, 920250},				//(15)
//	{"16--Australia 920-926(Australia)", 12, 500, 920250},				//(16)
//	{"17--India 865-867(India)", 4, 600, 865100},						//(17)
//	{"18--Uruguay 916-928(Uruguay)", 23, 500, 916250},					//(18)
//	{"19--Vietnam 920-925(Vietnam)", 10, 500, 920250},					//(19)
//	{"20--Israel 915-917", 1, 0, 916250},								//(20)
//	{"21--Philippines 918-920(Philippines)", 4, 500, 918250},			//(21)
//	{"22--Canada 902-928(Canada)", 42, 500, 902750},					//(22)
//	{"23--Indonesia 923-925(Indonesia)", 4, 500, 923250},				//(23)
//	{"24--New Zealand 921.5-928(New Zealand)", 11, 500, 922250},		//(24)
//};


        /// <summary>
        /// 
        /// </summary>
        public Reader()
        {
            
        }

        //==============================读写器应用函数定义==============================

        //连接读写器
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 ConnectScanner(ref int hScanner, string szPort, int nBaudRate);

        //断开连接
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 DisconnectScanner(int hScanner);

        //通过USB连接读写器
        //apiReturn _stdcall VH_ConnectScannerUsb(HANDLE *hScanner);
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 VH_ConnectScannerUsb(ref int hScanner);

        //断开连接
        //apiReturn _stdcall VH_DisconnectScannerUsb(HANDLE hScanner);
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 VH_DisconnectScannerUsb(int hScanner);

        //检测USB在线,bDech---1为在线，否则不在线
        //typedef int (_stdcall *VH_fDechUsb)(BOOL bDech);
        public delegate int VH_fDechUsb(bool bDech);

        //初始化USB，可检测，传入函数地址
        //apiReturn _stdcall VH_InitUsb(VH_fDechUsb fFunc);
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 VH_InitUsb(VH_fDechUsb fFunc);

        //关闭USB,释放资源
        //apiReturn _stdcall VH_CloseUsb();

        //////////////////////////////////////////////////////////////////////////
        //用途:		设置通信方式
        //函数名:	Mux_SetCommunicationOpt
        //功能:		对手持机的通信模式进行设置
        //输入参数:	lOpt 为何种方式通信(0--TCP/IP, 1--RS232,2--RS485, 3--USB)
        //			lpRHand 为读写器的句柄
        //输出参数:	
        //返回值:	返回 0 为成功，其它失败
        //备注:		外部接口
        //////////////////////////////////////////////////////////////////////////
        //apiReturn _stdcall VH_SetCommunicationOpt(long lOpt, void *lpRWHand);
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 VH_SetCommunicationOpt(int lOpt, int lpRWHand);

        //////////////////////////////////////////////////////////////////////////
        //用途:		设置输出日志文件
        //函数名:	VH_SetLogFile
        //功能:		将日志功能写入到指定文件中
        //输入参数:	FileName 指全路径名
        //输出参数:	无
        //返回值:	返回0为成功，否则-1为失败
        //备注:
        //////////////////////////////////////////////////////////////////////////
        //BOOL _stdcall VH_SetLogFile(char* FileName)
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 VH_SetLogFile(string FileName);

        //////////////////////////////////////////////////////////////////////////
        //用途:		写日志
        //函数名:	VH_WriteAppLogFile
        //功能:		将日志功能写入到指定文件中
        //输入参数:	FileName 指全路径名
        //			Line 指当前行号，这里暂不使用
        //			fmt 指格式化，你懂的
        //输出参数:	无
        //返回值:	返回0为成功，否则-1为失败
        //备注:
        //////////////////////////////////////////////////////////////////////////
        //int _stdcall VH_WriteAppLogFile(char* FileName, int Line, char* fmt, ...);
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern int VH_WriteAppLogFile(string FileName, int Line, string strOutput);

        //==============================上载控制命令==============================
        //读取版本号
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 VH_GetVersion(int hScanner, ref int wHardVer, ref int wSoftVer);
        //这样做的目的跟原来保持一致
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 GetVersion(int hScanner, int wHardVer, int wSoftVer);

        //启动/停止自动模式,0--stop, 1--start
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 VH_AutoMode(int hScanner, int Mode);

        //启动/停止自动模式,0--stop, 1--start
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 AutoMode(int hScanner, int Mode);

        //增加名单
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 AddLableID(int hScanner, int Count, int Len, byte data);

        //删除名单
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 DelLableID(int hScanner);

        //增加单个或多个名单
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 DelSingleLableID(int hScanner, int Count, int Len, byte data);

        //获得名单
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 GetLableID(int hScanner, int startaddr, int listlen, int nTotal, int DataLen, byte data);

        //提交名单
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 SaveLableID(int hScanner);

        //获取读写器基本工作参数
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 ReadHanderParam(int hScanner, HandsetParam pParam);

        //设置读写器基本工作参数
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 WriteHanderParam(int hScanner, HandsetParam pParam);

        //////////////////////////////////////////////////////////////////////////
        //用途:		取仿真键盘小模块参数
        //函数名:	ReadSimParam
        //功能:		将小模块的参数读出来
        //输入参数:	hScanner 为通信句柄,
        //输出参数:	pParam 为结构体指针
        //返回值:	返回 0 为成功，否则失败
        //备注:
        //////////////////////////////////////////////////////////////////////////
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 ReadSimParam(int hScanner, ref SimParam pParam);
        //public static extern UInt16 ReadSimParam(int hScanner, byte[] pParam);

        //////////////////////////////////////////////////////////////////////////
        //用途:		设置仿真键盘小模块参数
        //函数名:	WriteSimParam
        //功能:		将小模块的参数写入其中
        //输入参数:	hScanner 为通信句柄,pParam 为结构体指针
        //输出参数:	
        //返回值:	返回 0 为成功，否则失败
        //备注:
        //////////////////////////////////////////////////////////////////////////
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 WriteSimParam(int hScanner, ref SimParam pParam);

        //设置读写器基本工作参数
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 WriteBasicParam(int hScanner, ref ReaderBasicParam pParam);

        //读取读写器自动工作参数
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 ReadAutoParam(int hScanner, ref ReaderAutoParam pParam);

        //设置读写器自动工作参数
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 WriteAutoParam(int hScanner, ref ReaderAutoParam pParam);

        //设置厂工作参数
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 WriteFactoryParam(int hScanner, ref SimParam pParam);

        //恢复读写器出厂参数
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 ReadFactoryParameter(int hScanner);

        //获得读写器工作频率范围
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 GetFrequencyRange(int hScanner, ref byte Frequency);

        //获得读写器ID
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 GetReaderID(int hScanner, byte[] ReaderID);

        //设置时间
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 SetReaderTime(int hScanner, ReaderDate time);

        //获得时间
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 GetReaderTime(int hScanner, ReaderDate time);

        //设置标签过滤器
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 SetReportFilter(int hScanner, int ptr, int len, byte mask);

        //获得标签过滤器
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 GetReportFilter(int hScanner, int ptr, int len, byte mask);

        //////////////////////////////////////////////////////////////////////////
        //用途:		获取时间段的记录总数
        //函数名:	VH_GetRecordNum
        //功能:		取记录总数
        //输入参数:	hScanner 通信句柄
        //			stime 为起始时间，etime 为结束时间
        //输出参数:	nTotal 为记录总数
        //返回值:	0--成功，其它失败
        //备注:		注意：如果起始时间和结束时间都为0,则取机器中全部记录。
        //////////////////////////////////////////////////////////////////////////
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 VH_GetRecordNum(int hScanner, ReaderDate stime, ReaderDate etime, int nTotal);

        //////////////////////////////////////////////////////////////////////////
        //用途:		获取时间段的记录
        //函数名:	VH_GetRecord
        //功能:		取记录
        //输入参数:	hScanner 通信句柄
        //			stime 为起始时间，etime 为结束时间
        //输出参数:	nTotal 为记录总数, nDiNum 为第几条，从0开始算, data 指记录指针
        //返回值:	0--成功，其它失败
        //备注:		注意：如果起始时间和结束时间都为0,则取机器中全部记录。
        //////////////////////////////////////////////////////////////////////////
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 VH_GetRecord(int hScanner, ReaderDate stime, ReaderDate etime, int nTotal, int nDiNum, byte data);

        //获得记录
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 GetRecord(int hScanner, ReaderDate stime, ReaderDate etime, int nTotal, int taglen, byte data);

        //删除全部记录
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 DeleteAllRecord(int hScanner);

        //设置读写器ID
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 SetHandsetID(int hScanner, byte HandsetID);

        //获得读写器ID
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 GetHandsetID(int hScanner, byte HandsetID);
        

        //==============================EPC C1G2数据读写命令==============================
        //读取EPC1G2标签ID号
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 EPC1G2_ReadLabelID(int hScanner, int mem, int ptr, int len, byte[] mask, byte[] IDBuffer, ref int nCounter, int Address);

        //读一块数据
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 EPC1G2_ReadWordBlock(int hScanner, byte EPC_WORD, byte[] IDBuffer, byte mem, byte ptr, byte len, byte[] Data, byte[] AccessPassword, int Address);

        //写一块数据
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 EPC1G2_WriteWordBlock(int hScanner, byte EPC_WORD, byte[] IDBuffer, byte mem, byte ptr, byte len, byte[] Data, byte[] AccessPassword, int Address);

        //写EPC
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 EPC1G2_WriteEPC(int hScanner, byte len, byte[] Data, byte[] AccessPassword, int Address);

        //永久休眠标签
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 EPC1G2_KillTag(int hScanner, byte EPC_WORD, byte[] IDBuffer, byte[] KillPassword, int Address);
       
        //蜂鸣器报警,0--stop, 1--start
        //apiReturn _stdcall BuzzerAlarm(HANDLE hScanner,int iAlarm);
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 BuzzerAlarm(int hScanner, int iAlarm);

        //设置读写保护状态
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 EPC1G2_SetLock(int hScanner, byte EPC_WORD, byte[] IDBuffer, byte mem, byte Lock, byte[] AccessPassword, int Address);
        
        //读取读写器基本工作参数
        //apiReturn _stdcall ReadBasicParam(HANDLE hScanner, ReaderBasicParam * pParam);
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 ReadBasicParam(int hScanner, ref ReaderBasicParam pParam);

        //设置读写器基本工作参数
        //apiReturn _stdcall WriteBasicParam(HANDLE hScanner, ReaderBasicParam * pParam);
        //[DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        //public static extern UInt16 WriteBasicParam(int hScanner, ref ReaderBasicParam pParam);

        //EAS状态操作命令
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 EPC1G2_ChangeEas(int hScanner, byte EPC_WORD, byte[] IDBuffer, byte State, byte[] AccessPassword, int Address);
        //EAS报警命令
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern UInt16 EPC1G2_EasAlarm(int hScanner, int Address);

        /* ans = first + second */ 
        //实现两个16进制字符串的大数相加,如：
        // first:ABCDEF123456ABCDEF123456ABCDEF123456
        // second:ABCDEF123456ABCDEF123456ABCDEF123456
        //结果存入 ans:1579BDE2468AD579BDE2468AD579BDE2468AC
        //void _stdcall VH_big_num_add(char *ans, char *first, char *second);
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern void VH_big_num_add(StringBuilder ans, string first, string second);

        //////////////////////////////////////////////////////////////////////////
        //用途:		Bcd码转成ASC码并补空格
        //函数名:	Bcd2AscEx
        //功能:		将BCD码转成ASC码,即16进制转成字符串
        //输入参数:	bcd 指16进制的指针
        //			len 指其长度
        //输出参数:	bcd 指转后输出的指针
        //返回值:	无
        //备注:		与Bcd2Asc不同的是多出空格，如0x40 0x03==>40 03 ,而不是4003
        //////////////////////////////////////////////////////////////////////////
        //void _stdcall Bcd2AscEx(char *asc, char *bcd, int len)
        [DllImport(PUBLIC_COM_PATH, CharSet = CharSet.Ansi)]
        public static extern void Bcd2AscEx(StringBuilder asc, byte[] bcd, int len);
    }
}
