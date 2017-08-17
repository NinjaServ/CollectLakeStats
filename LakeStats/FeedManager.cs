using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Collections.Specialized; 
//Microsoft.VisualBasic.FileIO.TextFieldParser; 

namespace LakeStats
{

    /// <summary>
    /// FeedManager class establishes connection to Lake Pend Oreille weather data and gets 3 parameter data sets.
    /// Navy website is now offline
    /// </summary>
    class FeedManager
    {
        public const string WIND_SPEED = "Wind_Speed";
        public const string BARO_PRESSURE = "Barometric_Press";
        public const string AIR_TEMP = "Air_Temp";

        private const string BASE_URL = "https://lpo.dt.navy.mil/data/DM/";
        private const string BASE_PATH = @"c:\lakestats\DataFiles\";


        List<double> values;
        CacheManager cacheManager;

        /// <summary>
        /// Constructor creates new CacheManager object
        /// </summary>
        public FeedManager()
        {
            cacheManager = new CacheManager();
        }

        /// <summary>
        /// test
        /// </summary>
        /// <param name="thisDate">56</param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public FeedResult GetData(DateTime thisDate, string dataType)
        {
            //Initialize variables
            values = new List<double>();
            List<string> lines;
            FeedResult result = new FeedResult();

            //First try to get text lines from cache
            lines = cacheManager.getDataFromCache(thisDate, dataType);
            //If not found, pull from the web feed
            if (lines == null)
            {
                Console.WriteLine("Downloading: " + thisDate.ToString("MM/dd/yyyy"));
                //lines = GetDataFromURL(thisDate, dataType);
                lines = GetDataFromFile(thisDate, dataType);
            }
            else
            {
                Console.WriteLine("From cache : " + thisDate.ToString("MM/dd/yyyy"));
            }

            //Process the lines of text
            foreach (string line in lines)
            {
                //Get the 3rd item in the list, add to arraylist
                char[] separators = new char[] { ' ' };
                string[] words = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                values.Add(Convert.ToDouble(words[2]));

            }

            //Report the number of items in the list
            result.count = values.Count;

            //Now calculate and package the results
            //Start with mean, rounded to 2 decimals.
            result.mean = values.Average();

            //Calculate the median value; first sort the values in ascending order
            values.Sort();

            //Get the median index as a double
            double medianIndex = (double)(values.Count - 1) / 2;

            //Check whether median index is fractional or whole, get the value
            if (Math.Truncate(medianIndex) == medianIndex)
            {
                result.median = values[Convert.ToInt32(medianIndex)];
            }
            else
            {
                int lowIndex = (Int32)Math.Truncate(medianIndex);
                int highIndex = lowIndex + 1;
                result.median = ((values[lowIndex] + values[highIndex]) / 2);
            }

            return result;
        }

        private List<string> GetDataFromURL(DateTime thisDate, string dataType)
        {
            //Build URL from base + year + formatted date
            string feedURL = BASE_URL +
                thisDate.ToString("yyyy") + "/" +
                thisDate.ToString("yyyy_MM_dd") + "/" +
                dataType;

            WebResponse response = null;
            List<string> lines;

            try
            {
                lines = new List<string>();
                WebRequest request = WebRequest.Create(feedURL);
                response = request.GetResponse();

                Stream dataStream = response.GetResponseStream();
                StringWriter sw = new StringWriter();

                using (StreamReader reader = new StreamReader(dataStream))
                {
                    while (!reader.EndOfStream)
                    {
                        //get the next available line
                        string line = reader.ReadLine();
                        lines.Add(line);
                        sw.WriteLine(line);
                    }

                }
                cacheManager.writeFileToCache(sw.ToString(), thisDate, dataType);
            }
            catch (Exception e)
            {
                // If we get here, something went wrong - throw the exception to the calling scope
                throw (e);
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }

            return lines;
        }

        public List<string> GetDataFromFile(DateTime thisDate, string dataType)
        {
            //Build path from base + year + formatted date
            string feedPATH = BASE_PATH + "Environmental_Data_Deep_Moor_" + thisDate.ToString("yyyy") + ".txt";
            //thisDate.ToString("yyyy_MM_dd") + "/" + dataType;

            //string file = getFileName(thisDate, dataType);

            if (!File.Exists(feedPATH))
            {
                return null;
            }
            else
            {
                OrderedDictionary listdict = new OrderedDictionary();
                var dict = new Dictionary<string, List<string>>();
                var typelist = new List<string>();
                var datalist = new List<string>();
                List<string> lines;
                //var list = new List<string>();
                using (FileStream fs = File.OpenRead(feedPATH))
                {
                    StreamReader reader = new StreamReader(fs);

                    for (int count = 0; !reader.EndOfStream; count++)
                    {
                        //get the next available line
                        List<string> lineList = reader.ReadLine().Split('\t').ToList<string>();
                        if (count == 0)
                        {
                            foreach (var element in lineList)
                            {
                                listdict.Add(element, new List<string>());
                                dict.Add(element, new List<string>());
                                typelist.Add(element);
                            }

                        }
                        else
                        {
                            for (int index = 0; index < lineList.Count(); index++)
                            {
                                List<string> alist = (List<string>)listdict[index];
                                alist.Add(lineList[index]);
                                datalist.Add(lineList[index]);
                            }
                        }
                    }

                    int typeIndex = 0;
                    if (dataType.Equals(WIND_SPEED))
                        typeIndex = 7;
                    else if (dataType.Equals(BARO_PRESSURE))
                        typeIndex = 2;
                    else if (dataType.Equals(AIR_TEMP))
                        typeIndex = 1;
               
                    try
                    {
                        lines = new List<string>();
                        StringWriter sw = new StringWriter();

                        if (listdict.Count != 0)
                        {
                            string dateString = thisDate.ToString("yyyy_MM_dd");

                            List<string> dateList = (List<string>)listdict[0];
                            List<string> dataList = (List<string>)listdict[typeIndex];
                            foreach (string element in dateList)
                            {
                                
                                if ( element.Contains(dateString))
                                {
                                    int dateIndex = dateList.IndexOf(element);
                                    string dataString = dataList[dateIndex];
                                    //get the next available line
                                    string line = $"{element} {dataString}"; // string interpolation
                                    lines.Add(line);
                                    sw.WriteLine(line);
                                }

                            }

                        }
                        cacheManager.writeFileToCache(sw.ToString(), thisDate, dataType);
                    }
                    catch (Exception e)
                    {
                        // If we get here, something went wrong - throw the exception to the calling scope
                        throw (e);
                    }
                    finally
                    {
                        Console.WriteLine($"End of writing to {dataType} Cache file for {thisDate.ToString("yyyy_MM_dd")} from local file {feedPATH}"); 
                    }

                    return lines;

                }
            }
        }
    }
}

//public static void ReadTextFile()
//{
//    string line;

//    // Read the file and display it line by line.
//    using (StreamReader file = new StreamReader(@"C:\Documents and Settings\Administrator\Desktop\snpprivatesellerlist.txt"))
//    {
//        while ((line = file.ReadLine()) != null)
//        {

//            char[] delimiters = new char[] { '\t' };
//            string[] parts = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
//            for (int i = 0; i < parts.Length; i++)
//            {

//                Console.WriteLine(parts[i]);
//                sepList.Add(parts[i]);

//            }

//        }

//        file.Close();
//    }
//    // Suspend the screen.
//    Console.ReadLine();
//}