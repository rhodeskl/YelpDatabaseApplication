using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using Npgsql;

namespace JSON_Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            parseJson();
            populateDatabase();
        }

        static void parseJson()
        {
            string path = "C:\\Users\\Kayla Rhodes\\Documents\\College\\Senior Spring Semester Round 2\\CPTS 451\\Yelp-CptS451-2018\\yelp_business.JSON";
            //string path = "C:\\Users\\shrun\\Documents\\Course work\\CPTS 451\\Yelp-CptS451-2018\\yelp_business.JSON";
            StreamReader file;
            StreamWriter outFile;
            JObject obj;
            string line;
            file = new StreamReader(path);
            outFile = new StreamWriter("C:\\Users\\shrun\\Documents\\Course work\\CPTS 451\\CPTS451\\DatabaseProject\\JSON Parser\\outfile_business.txt");
            outFile = new StreamWriter("C:\\Users\\Kayla Rhodes\\Documents\\College\\Senior Spring Semester Round 2\\CPTS 451\\CPTS451\\DatabaseProject\\JSON Parser\\outfile_business.txt");
            Console.WriteLine("Parsing Business JSON");
            //outFile.WriteLine("yelp_business.JSON:");
            //outFile.WriteLine("business_id, name, address, state, city, postal_code, latitude, longitude, stars, review_count, is_open, [categories], [hours]");
            while ((line = file.ReadLine()) != null)
            {
                string temp;
                JObject attributes;
                JObject music;
                obj = JObject.Parse(line);
                outFile.Write("\'");
                outFile.Write(obj.GetValue("business_id"));
                outFile.Write("\'");
                outFile.Write(",");
                outFile.Write("\'");
                //outFile.Write(obj.GetValue("name"));
                string temp2 = obj.GetValue("name").ToString();
                List<string> pieces = temp2.Split(new char[] { '\'', ',' }).ToList();
                string name = "";
                foreach (string piece in pieces)
                {
                    name = name + piece;
                }
                outFile.Write(name);
                outFile.Write("\'");
                outFile.Write(",");
                outFile.Write("\'");
                //outFile.Write(obj.GetValue("address"));
                temp2 = obj.GetValue("address").ToString();
                pieces = temp2.Split(new char[] { '\'', ',' }).ToList();
                string address = "";
                foreach (string piece in pieces)
                {
                    address = address + piece;
                }
                outFile.Write(address);
                outFile.Write("\'");
                outFile.Write(",");
                outFile.Write("\'");
                outFile.Write(obj.GetValue("state"));
                outFile.Write("\'");
                outFile.Write(",");
                outFile.Write("\'");
                outFile.Write(obj.GetValue("city"));
                outFile.Write("\'");
                outFile.Write(",");
                outFile.Write("\'");
                outFile.Write(obj.GetValue("postal_code"));
                outFile.Write("\'");
                outFile.Write(",");
                outFile.Write(obj.GetValue("latitude"));
                outFile.Write(",");
                outFile.Write(obj.GetValue("longitude"));
                outFile.Write(",");
                outFile.Write(obj.GetValue("stars"));
                outFile.Write(",");
                outFile.Write(obj.GetValue("review_count"));
                outFile.Write(",");
                //outFile.Write(obj.GetValue("is_open"));
                string temp3 = obj.GetValue("is_open").ToString();
                if (temp2 == "0")
                {
                    outFile.Write("false");
                }
                else
                {
                    outFile.Write("true");
                }
                outFile.Write("\n");
                outFile.Write("Categories: [");
                temp = obj.GetValue("categories").ToString();
                temp = temp.Substring(1, temp.Length - 2);
                List<string> categories = temp.Split(new char[] { ',' }).ToList();
                foreach (string category in categories)
                {
                    if (category.Length > 0)
                    {
                        //get rid of junk characters in the start and end of the string before printing
                        string result;
                        char[] charsToTrimStart = { '\r', '\n', ' ', '\\' };
                        result = category.Trim(charsToTrimStart);
                        result = result.Substring(1, result.Length - 2);
                        outFile.Write("\'");
                        pieces = result.Split(new char[] { '\'' }).ToList();
                        result = "";
                        foreach (string piece in pieces)
                        {
                            result = result + piece;
                        }
                        outFile.Write(result);
                        outFile.Write("\'");
                        outFile.Write(", ");
                    }
                }
                outFile.Write("]\n");
                //    //temp = obj.GetValue("attributes").ToString();
                //    //attributes = JObject.Parse(temp);
                //    //outFile.Write("Attributes: [");
                //    //outFile.Write("(Alcohol, " + attributes.GetValue("Alcohol") + ")");
                //    //outFile.Write("(Caters, " + attributes.GetValue("Caters") + ")");
                //    //outFile.Write("(HasTV, " + attributes.GetValue("HasTV") + ")");
                //    //outFile.Write("(Noise Level, " + attributes.GetValue("NoiseLevel") + ")");
                //    //outFile.Write("(WiFi, " + attributes.GetValue("WiFi") + ")");
                //    //outFile.Write("(Restaurant Attire, " + attributes.GetValue("RestaurantsAttire") + ")");
                //    //outFile.Write("(Restaurant Reservations, " + attributes.GetValue("RestaurantReservations") + ")");
                //    //outFile.Write("(Outdoor Seating, " + attributes.GetValue("OutdoorSeating") + ")");
                //    //outFile.Write("(Business Accepts Credit Cards, " + attributes.GetValue("BusinessAcceptsCreditCards") + ")");
                //    //outFile.Write("(Restaurants Price Range, " + attributes.GetValue("RestaurantsPriceRange2") + ")");
                //    //outFile.Write("(Bike Parking, " + attributes.GetValue("BikeParking") + ")");
                //    //outFile.Write("(Restaurants Delivery, " + attributes.GetValue("RestaurantsDelivery") + ")");
                //    //outFile.Write("(Restaurants Take Out, " + attributes.GetValue("RestaurantsTakeOut") + ")");
                //    //outFile.Write("(Good For Kids, " + attributes.GetValue("GoodForKids") + ")");
                //    //if (attributes.GetValue("GoodForMeal") != null)
                //    //{
                //    //    temp = attributes.GetValue("GoodForMeal").ToString();
                //    //    JObject goodForMeal = JObject.Parse(temp);
                //    //    outFile.Write("(dessert, " + goodForMeal.GetValue("dessert") + ")");
                //    //    outFile.Write("(late night, " + goodForMeal.GetValue("latenight") + ")");
                //    //    outFile.Write("(lunch, " + goodForMeal.GetValue("lunch") + ")");
                //    //    outFile.Write("(dinner, " + goodForMeal.GetValue("dinner") + ")");
                //    //    outFile.Write("(breakfast, " + goodForMeal.GetValue("breakfast") + ")");
                //    //    outFile.Write("(brunch, " + goodForMeal.GetValue("brunch") + ")");
                //    //}
                //    //if (attributes.GetValue("Ambience") != null)
                //    //{
                //    //    temp = attributes.GetValue("Ambience").ToString();
                //    //    JObject ambience = JObject.Parse(temp);
                //    //    outFile.Write("(romantic, " + attributes.GetValue("romantic") + ")");
                //    //    outFile.Write("(intimate, " + attributes.GetValue("intimate") + ")");
                //    //    outFile.Write("(classy, " + attributes.GetValue("classy") + ")");
                //    //    outFile.Write("(hipster, " + attributes.GetValue("hipster") + ")");
                //    //    outFile.Write("(divey, " + attributes.GetValue("divey") + ")");
                //    //    outFile.Write("(touristy, " + attributes.GetValue("touristy") + ")");
                //    //    outFile.Write("(upscale, " + attributes.GetValue("upscale") + ")");
                //    //    outFile.Write("(casual, " + attributes.GetValue("casual") + ")");
                //    //}
                //    //if (attributes.GetValue("BusinessParking") != null)
                //    //{
                //    //    temp = attributes.GetValue("BusinessParking").ToString();
                //    //    JObject businessParking = JObject.Parse(temp);
                //    //    outFile.Write("(garage, " + businessParking.GetValue("garage") + ")");
                //    //    outFile.Write("(street, " + businessParking.GetValue("street") + ")");
                //    //    outFile.Write("(validated, " + businessParking.GetValue("validated") + ")");
                //    //    outFile.Write("(lot, " + businessParking.GetValue("lot") + ")");
                //    //    outFile.Write("(valet " + businessParking.GetValue("valet") + ")");
                //    //}
                //    //if (attributes.GetValue("Music") != null)
                //    //{
                //    //    temp = attributes.GetValue("Music").ToString();
                //    //    music = JObject.Parse(temp);
                //    //    outFile.Write("(dj, " + music.GetValue("dj") + ")");
                //    //    outFile.Write("(background music, " + music.GetValue("background_music") + ")");
                //    //    outFile.Write("(no music, " + music.GetValue("no_music") + ")");
                //    //    outFile.Write("(karaoke, " + music.GetValue("karaoke") + ")");
                //    //    outFile.Write("(live, " + music.GetValue("live") + ")");
                //    //    outFile.Write("(video, " + music.GetValue("video") + ")");
                //    //    outFile.Write("(jukebox, " + music.GetValue("jukebox") + ")");
                //    //}
                //    //outFile.Write("]\n");
                //    outFile.Write("Hours: [");
                //    temp = obj.GetValue("hours").ToString();
                //    obj = JObject.Parse(temp);
                //    outFile.Write("(Sunday," + obj.GetValue("Sunday") + ");");
                //    outFile.Write("(Monday," + obj.GetValue("Mo}nday") + ");");
                //    outFile.Write("(Tuesday," + obj.GetValue("Tuesday") + ");");
                //    outFile.Write("(Wednesday," + obj.GetValue("Wednesday") + ");");
                //    outFile.Write("(Thursday," + obj.GetValue("Thursday") + ");");
                //    outFile.Write("(Friday," + obj.GetValue("Friday") + ");");
                //    outFile.Write("(Saturday," + obj.GetValue("Saturday") + ")");
                //    outFile.Write("]\n");
            }
            file.Close();
            outFile.Close();

            outFile = new StreamWriter("C:\\Users\\shrun\\Documents\\Course work\\CPTS 451\\CPTS451\\DatabaseProject\\JSON Parser\\outfile_checkin.txt");
            outFile.WriteLine("yelp_checkin.JSON");
            outFile.WriteLine("business_id, dayofweek, morning, afternoon, evening, night");
            Console.WriteLine("Parsing checkin JSON");

            path = "C:\\Users\\shrun\\Documents\\Course work\\CPTS 451\\Yelp-CptS451-2018\\yelp_checkin.JSON";
            file = new StreamReader(path);
            //outFile.WriteLine();
            while ((line = file.ReadLine()) != null)
            {
                JObject time;
                JObject day;
                string temp;
                obj = JObject.Parse(line);
                outFile.Write(obj.GetValue("business_id"));
                temp = obj.GetValue("time").ToString();
                time = JObject.Parse(temp);
                outFile.Write("\n");
                if (time.GetValue("Monday") != null)
                {
                    day = JObject.Parse(time.GetValue("Monday").ToString());
                    outFile.Write("(\'Monday\', ");
                    printTimes(outFile, day);
                }
                if (time.GetValue("Tuesday") != null)
                {
                    day = JObject.Parse(time.GetValue("Tuesday").ToString());
                    outFile.Write("(\'Tuesday\', ");
                    printTimes(outFile, day);
                }
                if (time.GetValue("Wednesday") != null)
                {
                    day = JObject.Parse(time.GetValue("Wednesday").ToString());
                    outFile.Write("(\'Wednesday\', ");
                    printTimes(outFile, day);
                }
                if (time.GetValue("Thursday") != null)
                {
                    day = JObject.Parse(time.GetValue("Thursday").ToString());
                    outFile.Write("(\'Thursday\', ");
                    printTimes(outFile, day);
                }
                if (time.GetValue("Friday") != null)
                {
                    day = JObject.Parse(time.GetValue("Friday").ToString());
                    outFile.Write("(\'Friday\', ");
                    printTimes(outFile, day);
                }
                if (time.GetValue("Saturday") != null)
                {
                    day = JObject.Parse(time.GetValue("Saturday").ToString());
                    outFile.Write("(\'Saturday\', ");
                    printTimes(outFile, day);
                }
                if (time.GetValue("Sunday") != null)
                {
                    day = JObject.Parse(time.GetValue("Sunday").ToString());
                    outFile.Write("(\'Sunday\', ");
                    printTimes(outFile, day);
                }
                //outFile.WriteLine();
            }

            file.Close();
            outFile.Close();

            //outFile = new StreamWriter("C:\\Users\\shrun\\Documents\\Course work\\CPTS 451\\CPTS451\\DatabaseProject\\JSON Parser\\outfile_review.txt");
            outFile = new StreamWriter("C:\\Users\\Kayla Rhodes\\Documents\\College\\Senior Spring Semester Round 2\\CPTS 451\\CPTS451\\DatabaseProject\\JSON Parser\\outfile_review.txt");
            //outFile.WriteLine("yelp_review.JSON");
            //outFile.WriteLine("review_id, user_id, business_id, stars, date, text, useful, funny, cool");
            Console.WriteLine("Parsing review JSON");

            //path = "C:\\Users\\shrun\\Documents\\Course work\\CPTS 451\\Yelp-CptS451-2018\\yelp_review.JSON";
            path = "C:\\Users\\Kayla Rhodes\\Documents\\College\\Senior Spring Semester Round 2\\CPTS 451\\Yelp-CptS451-2018\\yelp_review.JSON";
            file = new StreamReader(path);
            outFile.WriteLine();
            string text = "";
            int lineNum = 1;
            while ((line = file.ReadLine()) != null)
            {
                Console.WriteLine("reading line number" + lineNum);
                obj = JObject.Parse(line);
                outFile.Write("\'" + obj.GetValue("review_id") + "\'");
                outFile.Write(",");
                outFile.Write("\'" + obj.GetValue("user_id") + "\'");
                outFile.Write(",");
                outFile.Write("\'" + obj.GetValue("business_id") + "\'");
                outFile.Write(",");
                outFile.Write(obj.GetValue("stars"));
                outFile.Write(",");
                outFile.Write("\'" + obj.GetValue("date") + "\'");
                outFile.Write(",");
                string temp4 = (string)obj.GetValue("text");
                temp4 = temp4.Replace(",", string.Empty);
                temp4 = temp4.Replace("\'", string.Empty);
                temp4 = temp4.Replace("\n", " ");
                temp4 = temp4.Replace("\r", " ");
                outFile.Write("\'" + temp4 + "\'");
                outFile.Write(",");
                bool use = (bool)obj.GetValue("useful");
                outFile.Write(use);
                outFile.Write(",");
                bool fun = (bool)obj.GetValue("funny");
                outFile.Write(fun);
                outFile.Write(",");
                bool cool = (bool)obj.GetValue("cool");
                outFile.Write(cool);
                outFile.Write("\n");
                lineNum++;
            }

            file.Close();
            outFile.Close();

            //outFile = new StreamWriter("C:\\Users\\shrun\\Documents\\Course work\\CPTS 451\\CPTS451\\DatabaseProject\\JSON Parser\\outfile_user.txt");
            outFile = new StreamWriter("C:\\Users\\Kayla Rhodes\\Documents\\College\\Senior Spring Semester Round 2\\CPTS 451\\CPTS451\\DatabaseProject\\JSON Parser\\outfile_user.txt");
            //outFile.WriteLine("yelp_user.JSON");
            //outFile.WriteLine("name, average_stars, fans, yelping_since, funny, cool, useful, friends, user_id, review_count");
            Console.WriteLine("Parsing user JSON");

            //path = "C:\\Users\\shrun\\Documents\\Course work\\CPTS 451\\Yelp-CptS451-2018\\yelp_user.JSON";
            path = "C:\\Users\\Kayla Rhodes\\Documents\\College\\Senior Spring Semester Round 2\\CPTS 451\\Yelp-CptS451-2018\\yelp_user.JSON";
            file = new StreamReader(path);
            outFile.WriteLine();
            while ((line = file.ReadLine()) != null)
            {
                obj = JObject.Parse(line);
                outFile.Write("\'" + obj.GetValue("user_id") + "\'");
                outFile.Write(",");
                string temp3 = obj.GetValue("name").ToString();
                List<string> pieces2 = temp3.Split(new char[] { '\'', ',' }).ToList();
                string name = "";
                foreach (string piece in pieces2)
                {
                    name = name + piece;
                }
                outFile.Write("\'" + name + "\'");
                outFile.Write(",");
                outFile.Write(obj.GetValue("average_stars"));
                outFile.Write(",");
                outFile.Write(obj.GetValue("fans"));
                outFile.Write(",");
                outFile.Write("\'" + obj.GetValue("yelping_since") + "\'");
                outFile.Write(",");
                outFile.Write(obj.GetValue("funny"));
                outFile.Write(",");
                outFile.Write(obj.GetValue("cool"));
                outFile.Write(",");
                outFile.Write(obj.GetValue("useful"));
                outFile.Write(",");
                string temp = obj.GetValue("friends").ToString();
                temp = temp.Substring(1, temp.Length - 2);
                List<string> friends = temp.Split(new char[] { ',' }).ToList();
                outFile.Write(obj.GetValue("review_count"));
                outFile.Write("\n");
                outFile.Write("Friends: ");
                foreach (string friend in friends)
                {
                    if (friend.Length > 0)
                    {
                        //get rid of junk characters in the start and end of the string before printing
                        string result;
                        char[] charsToTrimStart = { '\r', '\n', ' ', '\\' };
                        result = friend.Trim(charsToTrimStart);
                        result = result.Substring(1, result.Length - 2);
                        outFile.Write(result);
                        outFile.Write(",");
                    }
                }
                outFile.Write("\n");
            }

            file.Close();

            outFile.Close();
        }

        static void printTimes(StreamWriter outFile, JObject day)
        {
            int morningHours = 0, afternoonHours = 0, eveningHours = 0, nightHours = 0, hours;
            if (day.GetValue("0:00") != null)
            {
                hours = Int32.Parse(day.GetValue("0:00").ToString());
                nightHours = nightHours + hours;
            }
            if (day.GetValue("1:00") != null)
            {
                hours = Int32.Parse(day.GetValue("1:00").ToString());
                nightHours = nightHours + hours;
            }
            if (day.GetValue("2:00") != null)
            {
                hours = Int32.Parse(day.GetValue("2:00").ToString());
                nightHours = nightHours + hours;
            }
            if (day.GetValue("3:00") != null)
            {
                hours = Int32.Parse(day.GetValue("3:00").ToString());
                nightHours = nightHours + hours;
            }
            if (day.GetValue("4:00") != null)
            {
                hours = Int32.Parse(day.GetValue("4:00").ToString());
                nightHours = nightHours + hours;
            }
            if (day.GetValue("5:00") != null)
            {
                hours = Int32.Parse(day.GetValue("5:00").ToString());
                nightHours = nightHours + hours;
            }
            if (day.GetValue("6:00") != null)
            {
                hours = Int32.Parse(day.GetValue("6:00").ToString());
                morningHours = morningHours + hours;
            }
            if (day.GetValue("7:00") != null)
            {
                hours = Int32.Parse(day.GetValue("7:00").ToString());
                morningHours = morningHours + hours;
            }
            if (day.GetValue("8:00") != null)
            {
                hours = Int32.Parse(day.GetValue("8:00").ToString());
                morningHours = morningHours + hours;
            }
            if (day.GetValue("9:00") != null)
            {
                hours = Int32.Parse(day.GetValue("9:00").ToString());
                morningHours = morningHours + hours;
            }
            if (day.GetValue("10:00") != null)
            {
                hours = Int32.Parse(day.GetValue("10:00").ToString());
                morningHours = morningHours + hours;
            }
            if (day.GetValue("11:00") != null)
            {
                hours = Int32.Parse(day.GetValue("11:00").ToString());
                morningHours = morningHours + hours;
            }
            if (day.GetValue("12:00") != null)
            {
                hours = Int32.Parse(day.GetValue("12:00").ToString());
                afternoonHours = afternoonHours + hours;
            }
            if (day.GetValue("13:00") != null)
            {
                hours = Int32.Parse(day.GetValue("13:00").ToString());
                afternoonHours = afternoonHours + hours;
            }
            if (day.GetValue("14:00") != null)
            {
                hours = Int32.Parse(day.GetValue("14:00").ToString());
                afternoonHours = afternoonHours + hours;
            }
            if (day.GetValue("15:00") != null)
            {
                hours = Int32.Parse(day.GetValue("15:00").ToString());
                afternoonHours = afternoonHours + hours;
            }
            if (day.GetValue("16:00") != null)
            {
                hours = Int32.Parse(day.GetValue("16:00").ToString());
                afternoonHours = afternoonHours + hours;
            }
            if (day.GetValue("17:00") != null)
            {
                hours = Int32.Parse(day.GetValue("17:00").ToString());
                eveningHours = eveningHours + hours;
            }
            if (day.GetValue("18:00") != null)
            {
                hours = Int32.Parse(day.GetValue("18:00").ToString());
                eveningHours = eveningHours + hours;
            }
            if (day.GetValue("19:00") != null)
            {
                hours = Int32.Parse(day.GetValue("19:00").ToString());
                eveningHours = eveningHours + hours;
            }
            if (day.GetValue("20:00") != null)
            {
                hours = Int32.Parse(day.GetValue("20:00").ToString());
                eveningHours = eveningHours + hours;
            }
            if (day.GetValue("21:00") != null)
            {
                hours = Int32.Parse(day.GetValue("21:00").ToString());
                eveningHours = eveningHours + hours;
            }
            if (day.GetValue("22:00") != null)
            {
                hours = Int32.Parse(day.GetValue("22:00").ToString());
                eveningHours = eveningHours + hours;
            }
            if (day.GetValue("23:00") != null)
            {
                hours = Int32.Parse(day.GetValue("23:00").ToString());
                nightHours = nightHours + hours;
            }
            outFile.Write(morningHours + ", ");
            outFile.Write(afternoonHours + ", ");
            outFile.Write(eveningHours + ", ");
            outFile.Write(nightHours + ")\n");
        }

        static void populateDatabase()
        {

            //insert data to database
            string path;
            //business table, hours table, categories table
            Console.WriteLine("Begin Inserting into Business, Hours, and Categories tables");
            path = "C:\\Users\\shrun\\Documents\\Course work\\CPTS 451\\CPTS451\\DatabaseProject\\JSON Parser\\outfile_business.txt";
            StreamReader file;
            file = new StreamReader(path);
            string line;
            string business_id = "";
            //int numLine = 1;
            while ((line = file.ReadLine()) != null)
            {
                //Console.WriteLine("Current line number = " + numLine);
                if (line.StartsWith("Categories: "))
                {
                    //Console.WriteLine("Inserting into Category Table");
                    string temp = line.Substring(13);
                    temp = temp.TrimEnd(']');
                    List<string> categories = temp.Split(new char[] { ',' }).ToList();
                    using (var conn = new NpgsqlConnection(buildConnString()))
                    {
                        conn.Open();
                        using (var cmd = new NpgsqlCommand())
                        {
                            cmd.Connection = conn;
                            foreach (string category in categories)
                            {
                                if (category != " ")
                                {
                                    string pair = business_id + ',' + category.Trim('[');
                                    cmd.CommandText = "INSERT INTO Category(business_id,category) VALUES (" + pair + ")";
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        conn.Close();
                    }
                }
                else if (line.StartsWith("Hours: "))
                {
                    //Console.WriteLine("Inserting into Hours Table");
                    string temp = line.Substring(8);
                    temp = temp.TrimEnd(']');
                    List<string> hourPairs = temp.Split(new char[] { ';' }).ToList();
                    using (var conn = new NpgsqlConnection(buildConnString()))
                    {
                        conn.Open();
                        using (var cmd = new NpgsqlCommand())
                        {
                            cmd.Connection = conn;
                            foreach (string hourPair in hourPairs)
                            {
                                string temp2;
                                string day_of_week;
                                string start_hour = "null";
                                string end_hour = "null";
                                temp2 = hourPair.Trim('(');
                                temp2 = temp2.TrimEnd(')');
                                List<string> pairEntries = temp2.Split(new char[] { ',' }).ToList();
                                day_of_week = "\'" + pairEntries[0] + "\'";
                                if (pairEntries[1] != "")
                                {
                                    List<string> times = pairEntries[1].Split(new char[] { '-' }).ToList();
                                    start_hour = "\'" + times[0] + "\'";
                                    end_hour = "\'" + times[1] + "\'";
                                }
                                temp2 = day_of_week + ',' + business_id + ',' + start_hour + ',' + end_hour;
                                cmd.CommandText = "INSERT INTO Hours(day_of_week,business_id,start_hour,end_hour) VALUES (" + temp2 + ")";
                                cmd.ExecuteNonQuery();
                            }
                        }
                        conn.Close();
                    }
                }
                else
                {
                    //Console.WriteLine("Inserting into Business Table");
                    using (var conn = new NpgsqlConnection(buildConnString()))
                    {
                        conn.Open();
                        using (var cmd = new NpgsqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandText = "INSERT INTO Business(business_id,name,address,state_name,city,postal_code,latitude,longitude,stars,review_count,is_open,numcheckins,review_rating) VALUES (" + line + ",0,0)";
                            cmd.ExecuteNonQuery();
                        }
                        conn.Close();
                    }
                    int index = 0;
                    while (line[index] != ',')
                    {
                        index++;
                    }
                    business_id = line.Substring(0, index - 1) + "\'";
                }
                //numLine++;
            }
            Console.WriteLine("Successfully inserted business, hours, and category data");
            file.Close();

            //checkin table
            Console.WriteLine("Begin Inserting into Checkin table");
            path = "C:\\Users\\shrun\\Documents\\Course work\\CPTS 451\\CPTS451\\DatabaseProject\\JSON Parser\\outfile_checkin.txt";
            //StreamReader file;
            file = new StreamReader(path);
            business_id = "";
            //int lineNum = 0;
            while ((line = file.ReadLine()) != null)
            {
                //Console.WriteLine("Current line is " + lineNum);
                if (line.StartsWith("("))
                {
                    line = line.Trim('(');
                    line = line.TrimEnd(')');
                    using (var conn = new NpgsqlConnection(buildConnString()))
                    {
                        conn.Open();
                        using (var cmd = new NpgsqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandText = "INSERT INTO Checkin(business_id,day_of_week,morning,afternoon,evening,night) VALUES (" + business_id + "," + line + ")";
                            cmd.ExecuteNonQuery();
                        }
                        conn.Close();
                    }
                }
                else
                {
                    business_id = "\'" + line + "\'";
                }
                //lineNum++;
            }
            file.Close();

            //Users table
            Console.WriteLine("Begin Inserting into User table");
            //path = "C:\\Users\\Kayla Rhodes\\Documents\\College\\Senior Spring Semester Round 2\\CPTS 451\\CPTS451\\DatabaseProject\\JSON Parser\\outfile_user.txt";
            path = "C:\\Users\\shrun\\Documents\\Course work\\CPTS 451\\CPTS451\\DatabaseProject\\JSON Parser\\outfile_user.txt";
            //StreamReader file;
            file = new StreamReader(path);
            string user_id = "";
            //int lineNum = 0;

            while ((line = file.ReadLine()) != null)
            {
                if (line == "")
                {
                    continue;
                }
                if (line.StartsWith("Friends: "))
                {
                    continue;//do nothing because it would violate foreign key constraint
                }
                else
                {
                    //Console.WriteLine("Inserting into User Table");
                    using (var conn = new NpgsqlConnection(buildConnString()))
                    {
                        conn.Open();
                        using (var cmd = new NpgsqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandText = "INSERT INTO UserTable(user_id,name,avg_stars,fans,yelping_since,funny,cool,useful,review_count) VALUES (" + line + ")";
                            cmd.ExecuteNonQuery();
                        }
                        conn.Close();
                    }
                    int index = 0;
                    while (line[index] != ',')
                    {
                        index++;
                    }
                    user_id = line.Substring(0, index - 1);
                }

            }
            file.Close();

            Console.WriteLine("Begin Inserting into HasFriends table");
            //path = "C:\\Users\\shrun\\Documents\\Course work\\CPTS 451\\CPTS451\\DatabaseProject\\JSON Parser\\outfile_user.txt";
            path = "C:\\Users\\Kayla Rhodes\\Documents\\College\\Senior Spring Semester Round 2\\CPTS 451\\CPTS451\\DatabaseProject\\JSON Parser\\outfile_user.txt";
            //StreamReader file;
            file = new StreamReader(path);
            //int lineNum = 0;

            while ((line = file.ReadLine()) != null)
            {
                if (line == "")
                {
                    continue;
                }
                if (line.StartsWith("Friends: "))
                {
                    //Console.WriteLine("Inserting into Friends Table");
                    string temp = line.Substring(9);
                    List<string> friends = temp.Split(new char[] { ',' }).ToList();
                    using (var conn = new NpgsqlConnection("Host = localhost; Username = postgres; Password = StarWars3827; Database = postgres"))
                    {
                        conn.Open();
                        using (var cmd = new NpgsqlCommand())
                        {
                            cmd.Connection = conn;
                            foreach (string friend in friends)
                            {
                                if (friend != "")
                                {
                                    string pair = user_id + "\'" + ',' + "\'" + friend + "\'";
                                    cmd.CommandText = "INSERT INTO hasFriends(user_id,friend_id) VALUES (" + pair + ")";
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        conn.Close();
                    }
                }
                else
                {
                    int index = 0;
                    while (line[index] != ',')
                    {
                        index++;
                    }
                    user_id = line.Substring(0, index - 1);
                }

            }
            file.Close();

            //Review table
            Console.WriteLine("Begin Inserting into Review table");
            //path = "C:\\Users\\shrun\\Documents\\Course work\\CPTS 451\\CPTS451\\DatabaseProject\\JSON Parser\\outfile_review.txt";
            path = "C:\\Users\\Kayla Rhodes\\Documents\\College\\Senior Spring Semester Round 2\\CPTS 451\\CPTS451\\DatabaseProject\\JSON Parser\\outfile_review.txt";
            //StreamReader file;
            file = new StreamReader(path);
            // business_id = "";
            int lineNum = 1;

            while (((line = file.ReadLine()) != null))
            {
                Console.WriteLine("Inserting line number " + lineNum);
                if (line == "")
                {
                    continue;
                }
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO Review(review_id,user_id,business_id,stars,date_review,text,useful,funny,cool) VALUES (" + line + ")";
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
                lineNum++;
            }
            file.Close();


        }

        private static string buildConnString()
        {
            //return "Host = localhost; Username = postgres; Password = password;Database = milestone2";
            return "Host = localhost; Username = postgres; Password = StarWars3827; Database = postgres";
        }
    }
}