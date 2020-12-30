using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace taskUni
{
    public class Program
    {
        const int LOG_ERROR_CODE_INDEX = 4;
        const int ERROR_CODE_INDEX = 0;
        const int ERROR_CODE_DESCRIPTION_INDEX = 1;


        public string[] ReadData(string text)
        {
            string[] log = File.ReadAllLines(text);
            return log;
        }

        public List<string> MakeLogList(string logsDirPath)
        {
            var logList = new List<string>();
            foreach (var file in Directory.GetFiles(logsDirPath, "*.txt", SearchOption.AllDirectories))
            {
                string[] logs = File.ReadAllLines(file);
                logList.AddRange(logs);
            }
            return logList;
        }

        public Dictionary<string, string> MakeErrCodesDict(string errCodesPath)
        {
            string[] codes = ReadData(errCodesPath);
            var codeDict = new Dictionary<string, string>();
            foreach (string code in codes)
            {
                string[] splitted = code.Split('*');
                codeDict.Add(splitted[ERROR_CODE_INDEX], splitted[ERROR_CODE_DESCRIPTION_INDEX]); 
            }
            return codeDict;
        }

        public Dictionary<string, int> CountErrorOccurences(Dictionary<string, string> errCodes, List<string> logs)
        {
            var errCodesCounts = new Dictionary<string, int>();
            foreach (string errCode in errCodes.Keys)
            {
                int errCount = 0;
                foreach (string log in logs)
                {
                    string[] splittedLog = log.Split('*');
                    if (splittedLog[LOG_ERROR_CODE_INDEX] == errCode)
                    {
                        errCount++;
                    }
                }
                errCodesCounts.Add(errCode, errCount);
            }
            return errCodesCounts;
        }

        public void PrintData(Dictionary<string, int> errorCountDct, Dictionary<string, string> errorCodes)
        {
            foreach (KeyValuePair<string, int> err in errorCountDct)
            {
                System.Console.WriteLine("Error description: {0} \nError code: {1}  \nTotal Occurrence: {2}\n",errorCodes[err.Key] ,err.Key, err.Value);
            }
        }

        static void Main(string[] args)
        {
            string logDirPath = Environment.CurrentDirectory + "../../../../logs";
            Console.WriteLine(logDirPath);
            string errCodesPath = Environment.CurrentDirectory + "../../../../ABS_ErrorCodes.txt";

            Program fileHandler = new Program();

            Dictionary<string, string> errCodes = fileHandler.MakeErrCodesDict(errCodesPath);
            List<string> logs = fileHandler.MakeLogList(logDirPath);
            Dictionary<string, int> errCodeCount = fileHandler.CountErrorOccurences(errCodes, logs);

            Dictionary<string, int> orderedCodeCount =  errCodeCount.OrderByDescending(x => x.Value).Where(x => x.Value > 0).ToDictionary(x => x.Key, x => x.Value);

            fileHandler.PrintData(orderedCodeCount, errCodes);


            //Console.WriteLine(codes[0]);


        }
    }
}
