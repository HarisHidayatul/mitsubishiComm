using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mitsubishiComm
{
    class frameMitsubishi
    {
        //this code refrence to manual refrence fx-232aw
        //created by haris hidayatulloh - indonesia
        const byte stx = 0x02;
        const byte etx = 0x03;
        public string a;
        public List<byte> sendtoread(int command, char register, int address, int n)
        {
            List<byte> datasend = new List<byte>();
            if(register == 'D'||register=='d')
            {
                datasend.AddRange(inttoHexinByte(command,1));
                datasend.AddRange(inttoHexinByte(0x1000+(address*2),4));
                datasend.AddRange(inttoHexinByte(n,2));
            }
            datasend.Add(etx);
            datasend.AddRange(sumByte(datasend.ToArray()));
            datasend.Insert(0, stx);
            return datasend;
        }
        public bool checkSum(byte[] getbyte)
        {
            int length = getbyte.Length;
            byte[] newByte = new byte[length];
            byte[] checkSum = new byte[2];
            if(length > 2)
            {
                for(int i = 0; i < (length - 2); i++)
                {
                    newByte[i] = getbyte[i];
                }
                checkSum = sumByte(newByte);
                if (checkSum[0] == getbyte[length - 2])
                {
                    if(checkSum[1] == getbyte[length - 1])
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public List<int> readRespon(byte[] getByte)
        {
            List<int> valRespoon = new List<int>();
            int length = getByte.Length-3;
            if (checkSum(getByte))
            {
                if (length % 2 == 0)
                {
                    for(int i = 0; i < length / 2; i++)
                    {
                        valRespoon.Add(hexToInt(getByte,i*2,2));
                    }
                    return valRespoon;
                }
                else
                {
                    return valRespoon;
                }
            }
            else
            {
                return valRespoon;
            }
        }
        public int hexToInt(byte[] getByte, int start, int length)
        {
            int sum = 0;
            if (start+length <= getByte.Length)
            {
                length--;
                for(int i = 0; i <= length; i++)
                {
                    sum = sum + hex1dtoInt((char)getByte[start+i])*exponent(16,length-i);
                }
                return sum;
            }
            else
            {
                return 0;
            }
        }
        public int hex1dtoInt(char gethex)
        {
            if ((gethex > 48) && (gethex < 58)) 
            { 
                return gethex - 48; 
            }else if ((gethex > 64) && (gethex < 71))
            {
                return gethex - 55;
            }
            else
            {
                return 0;
            }
        }
        public char int1dtoHex(int getint)
        {
            getint = getint % 16;
            if(getint < 10) { return (char)(getint + 48); }
            else { return (char)(getint%10 + 65); }
        }
        public int exponent(int getInt,int n)
        {
            int sum = 1;
            for(int i = 0; i < n; i++)
            {
                sum = sum*getInt;
            }
            return sum;
        }
        public List<byte> inttoHexinByte(int getInt,int length)
        {
            List<byte> hex = new List<byte>();
            for (int i = length-1; i >= 0; i--)
            {
                //hex.Add((byte)((getInt / exponent(16, i))%16));
                hex.Add((byte)int1dtoHex(getInt/exponent(16,i)));
            }
            return hex;
        }
        public byte[] sumByte(byte[] getByte)
        {
            int sum = 0;
            for(int i=0;i<getByte.Length;i++)
            {
                sum = sum + getByte[i];
            }
            //convert byte to hex string
            string s = BitConverter.ToString(new byte[] { (byte)(sum % 256) });
            return new byte[] { (byte)s[0],(byte)s[1]};
        }
    }
}
