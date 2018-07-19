using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;
using System.Device.Location;

namespace DatabaseUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Business
        {
            public string bid { get; set; }
            public string name { get; set; }
            public string address { get; set; }
            //public List<string> categories { get; set; }
            public string state { get; set; }
            public string city { get; set; }
            //distance
            public double distance { get; set; }
            //stars
            public int stars { get; set; }
            //numreviews
            public int numReviews { get; set; }
            //average rating of reviews
            public double avg_rating_review { get; set; }
            //total checkins
            public int numCheckins { get; set; }
        }

        public class Review_Display
        {
            public string date { get; set; }
            public string userName { get; set; }
            public int stars{ get; set; }
            public string text { get; set; }
            
        }
        public class Review
        {
            public string userName { get; set; }
            public string business { get; set; }
            public string city { get; set; }
            public string text { get; set; }
            public double distance { get; set; }
        }

        public class Friends
        {
            public string name { get; set; }
            public string avgStars { get; set; }
            public string yelpingSince { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
            addStates();
            addColumnsToGrid();
            addColumnsToFriendTipsGrid();
            addColumnsToFriendDataGrid();
            addDays();
            addHours();
            addFilters();
            addRatings();
            addColumnsToReviewGrid();
            addDaysCheckin();
        }

        private string buildConnString()
        {
            //return "Host = localhost; Username = postgres; Password = password;Database = milestone2";
            return "Host=localhost; Username=postgres; Password=StarWars3827; Database= postgres";
        }

        public void addDaysCheckin()
        {
            checkinDayChart.Items.Add("Default");
            checkinDayChart.Items.Add("Sunday");
            checkinDayChart.Items.Add("Monday");
            checkinDayChart.Items.Add("Tuesday");
            checkinDayChart.Items.Add("Wednesday");
            checkinDayChart.Items.Add("Thursday");
            checkinDayChart.Items.Add("Friday");
            checkinDayChart.Items.Add("Saturday");
        }

        //a. Retrieve all distinct states that appear in the Yelp business data and list them
        public void addStates()
        {
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT DISTINCT state_name FROM Business;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            stateList.Items.Add(reader.GetString(0));
                        }
                    }
                }

                conn.Close();
            }

        }
        public void addRatings()
        {
            AddRatingBox.Items.Add("1");
            AddRatingBox.Items.Add("2");
            AddRatingBox.Items.Add("3");
            AddRatingBox.Items.Add("4");
            AddRatingBox.Items.Add("5");
            
        }

        public void addDays()
        {
            dayList.Items.Add("Sunday");
            dayList.Items.Add("Monday");
            dayList.Items.Add("Tuesday");
            dayList.Items.Add("Wednesday");
            dayList.Items.Add("Thursday");
            dayList.Items.Add("Friday");
            dayList.Items.Add("Saturday");
        }

        public void addHours()
        {
            string [] hours = new string []{ "00:00","01:00","02:00", "03:00", "04:00", "05:00", "06:00", "07:00", "08:00", "09:00", "10:00", "11:00"
                ,"12:00","13:00","14:00","15:00","16:00","17:00","18:00","19:00","20:00","21:00","22:00","23:00"};

            foreach ( string s in hours)
            {
                hours_s.Items.Add(s);
                hours_e.Items.Add(s);
            }
           
        }

        public void addFilters()
        {
            sortBy.Items.Add("Business Name");
            sortBy.Items.Add("Highest Rating");
            sortBy.Items.Add("Most Reviewed");
            sortBy.Items.Add("Best Review Rating");
            sortBy.Items.Add("Most Checkins");
            sortBy.Items.Add("Nearest");
        }

        //b. When user selects a state, retrieve and display the cities that appear in the Yelp business data in the selected state

        private void stateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            businessGrid.Items.Clear();
            cityList.Items.Clear();
            ZipList.Items.Clear();

            if (stateList.SelectedIndex > -1)
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT DISTINCT city FROM business WHERE state_name = '" +
                                          stateList.SelectedItem.ToString() + "' ORDER BY city;";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cityList.Items.Add(reader.GetString(0));
                            }
                        }
                    }

                    conn.Close();
                }
            }
        }

        private void cityList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            businessGrid.Items.Clear();
            ZipList.Items.Clear();

            if (cityList.SelectedIndex > -1)
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "select distinct postal_code from business where city = '" +
                                          cityList.SelectedItem.ToString() + "'; ";
                        //"SELECT name,city,state FROM business WHERE city = '" + cityList.SelectedItem.ToString() + "'AND state = '" + stateList.SelectedItem.ToString() + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ZipList.Items.Add(reader.GetString(0));
                                //ZipList.Items.Add(new Business() { name = reader.GetString(0), city = reader.GetString(1), state = reader.GetString(2) });
                            }
                        }
                    }

                    conn.Close();
                }
            }
        }
        
        private void btSearchBusiness_Click(object sender, RoutedEventArgs e)
        {
            if (lvSelectedctgry.Items.Count > 0)
            {
                search();
            }
            else
            {
                searchNoCategories();
            }

        }

        private void searchNoCategories()
        {
            businessGrid.Items.Clear();
            if (ZipList.SelectedIndex > -1)
            {
                string state = stateList.SelectedItem.ToString();
                string city = cityList.SelectedItem.ToString();
                string zip = ZipList.SelectedItem.ToString();
                string qStr = "";
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        qStr = "SELECT  * FROM Business as B where " +
                               "B.state_name ='" + state +
                               "' AND B.city ='" + city + "' AND B.postal_code = '" + zip +"'";

                        string orderby = "";
                        if (sortBy.SelectedIndex > -1)
                        {
                            orderby = sortBy.SelectedItem.ToString();
                        }

                        switch (orderby)
                        {
                            default:
                                qStr += " order by name";
                                break;
                            case "Highest Rating":
                                qStr += " order by stars desc";
                                break;
                            case "Most Reviewed":
                                qStr += " order by review_count desc";
                                break;
                            case "Best Review Rating":
                                qStr += " order by review_rating desc";
                                break;
                            case "Most Checkins":
                                qStr += " order by numcheckins desc";
                                break;
                            //TODO: add nearest filter
                        }

                        cmd.CommandText = qStr;
                        double user_latitude = 0.0, user_longitude = 0.0;
                        if (latitudeBox.Text != "" && longitudeBox.Text != "")
                        {
                            user_latitude = Convert.ToDouble(latitudeBox.Text);
                            user_longitude = Convert.ToDouble(longitudeBox.Text);
                        }

                        using (var reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                double distanceAway = 0.0;
                                double businessLatitude = reader.GetDouble(9);
                                double businessLongitude = reader.GetDouble(10);
                                if (latitudeBox.Text != "" && longitudeBox.Text != "")
                                {
                                    var businessCoord = new GeoCoordinate(businessLatitude, businessLongitude);
                                    var userCoord = new GeoCoordinate(user_latitude, user_longitude);
                                    distanceAway = businessCoord.GetDistanceTo(userCoord) * 0.000621371;
                                }

                                businessGrid.Items.Add(new Business()
                                {
                                    bid = reader.GetString(0),
                                    name = reader.GetString(1),
                                    address = reader.GetString(3),
                                    state = reader.GetString(5),
                                    city = reader.GetString(6),
                                    //distance
                                    distance = distanceAway,
                                    //stars
                                    stars = reader.GetInt32(11),
                                    //numreviews
                                    numReviews = reader.GetInt32(12),
                                    //average rating of reviews
                                    avg_rating_review = reader.GetDouble(2),
                                    //total checkins
                                    numCheckins = reader.GetInt32(4)

                                });
                            }
                            if (orderby == "Nearest")
                            {
                                //businessGrid.Items.SortDescriptions.Add(new SortDescription("Distance", ListSortDirection.Ascending));
                                //businessGrid.Items.Refresh();
                                var column = businessGrid.Columns[4];

                                // Clear current sort descriptions
                                businessGrid.Items.SortDescriptions.Clear();

                                // Add the new sort description
                                businessGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, ListSortDirection.Ascending));

                                // Apply sort
                                foreach (var col in dataGrid.Columns)
                                {
                                    col.SortDirection = null;
                                }
                                column.SortDirection = ListSortDirection.Ascending;

                                // Refresh items to display sort
                                dataGrid.Items.Refresh();
                            }
                        }
                    }
                }

            }
        }

        private void search()
        {
            businessGrid.Items.Clear();
            if (ZipList.SelectedIndex > -1)
            {
                string state = stateList.SelectedItem.ToString();
                string city = cityList.SelectedItem.ToString();
                string zip = ZipList.SelectedItem.ToString();
                string qStr = "";

                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;

                        qStr = "SELECT  * FROM Business where " +
                               "business.business_id in " +
                               "(select distinct temp1.business_id from" +
                               " (SELECT DISTINCT B.name, B.business_id FROM Business B, Category C " +
                               "WHERE B.business_id = C.business_id " +
                               "AND B.state_name='" + state + "' AND B.city ='" + cityList.SelectedItem.ToString() + "' AND B.postal_code = '" +
                               zip + "' AND C.category= '" + lvSelectedctgry.Items[0] + "') AS temp1 ";

                        for (int i = 1; i < lvSelectedctgry.Items.Count; i++)
                        {
                            qStr += "INNER JOIN (SELECT DISTINCT B.name, B.business_id" +
                                    " FROM Business B, Category C " +
                                    "WHERE B.business_id = C.business_id " +
                                    "AND B.state_name ='" + state +
                                    "' AND B.city ='" + city + "' AND B.postal_code = '" + zip +
                                    "' AND C.category = '" + lvSelectedctgry.Items[i] + "') AS temp" +
                                    (i + 1).ToString() + " ";
                        }

                        if (lvSelectedctgry.Items.Count > 1)
                        {
                            qStr += "ON ";

                            for (int i = 0; i < lvSelectedctgry.Items.Count - 1; i++)
                            {
                                qStr += "temp" + (i + 1).ToString() + ".name=" + "temp" + (i + 2).ToString() + ".name ";
                                if (i == lvSelectedctgry.Items.Count - 2)
                                {
                                    qStr += "";
                                }
                                else qStr += "AND ";
                            }
                        }

                        qStr += ")";
                        string orderby = "";
                        if (sortBy.SelectedIndex > -1)
                        {
                           orderby= sortBy.SelectedItem.ToString();
                        }

                        switch (orderby)
                        {
                            default:
                                qStr += " order by name";
                                break;
                            case "Highest Rating":
                                qStr += " order by stars desc";
                                break;
                            case "Most Reviewed":
                                qStr += " order by review_count desc";
                                break;
                            case "Best Review Rating":
                                qStr += " order by review_rating desc";
                                break;
                            case "Most Checkins":
                                qStr += " order by numcheckins desc";
                                break;
                            //TODO: add nearest filter
                        }
                        cmd.CommandText = qStr;
                        double user_latitude = 0.0, user_longitude = 0.0;
                        if (latitudeBox.Text != "" && longitudeBox.Text != "")
                        {
                            user_latitude = Convert.ToDouble(latitudeBox.Text);
                            user_longitude = Convert.ToDouble(longitudeBox.Text);
                        }
                        using (var reader = cmd.ExecuteReader())
                        {
                            
                            while (reader.Read())
                            {
                                double distanceAway = 0.0;
                                double businessLatitude = reader.GetDouble(9);
                                double businessLongitude = reader.GetDouble(10);
                                if (latitudeBox.Text != "" && longitudeBox.Text != "")
                                {
                                    var businessCoord = new GeoCoordinate(businessLatitude, businessLongitude);
                                    var userCoord = new GeoCoordinate(user_latitude, user_longitude);
                                    distanceAway = businessCoord.GetDistanceTo(userCoord) * 0.000621371;
                                }

                                businessGrid.Items.Add(new Business()
                                {
                                    bid = reader.GetString(0),
                                    name = reader.GetString(1),
                                    address = reader.GetString(3),
                                    state = reader.GetString(5),
                                    city = reader.GetString(6),
                                    //distance
                                    distance = distanceAway,
                                    //stars
                                    stars = reader.GetInt32(11),
                                    //numreviews
                                    numReviews = reader.GetInt32(12),
                                    //average rating of reviews
                                    avg_rating_review = reader.GetDouble(2),
                                    //total checkins
                                    numCheckins = reader.GetInt32(4)

                                });
                            }

                            if (orderby == "Nearest")
                            {
                                //businessGrid.Items.SortDescriptions.Add(new SortDescription("Distance", ListSortDirection.Ascending));
                                //businessGrid.Items.Refresh();
                                var column = businessGrid.Columns[4];

                                // Clear current sort descriptions
                                businessGrid.Items.SortDescriptions.Clear();

                                // Add the new sort description
                                businessGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, ListSortDirection.Ascending));

                                // Apply sort
                                foreach (var col in dataGrid.Columns)
                                {
                                    col.SortDirection = null;
                                }
                                column.SortDirection = ListSortDirection.Ascending;

                                // Refresh items to display sort
                                dataGrid.Items.Refresh();
                            }
                        }
                    }
                }

            }
        }

        private void filterByHoursNoCategory()
        {
            businessGrid.Items.Clear();
            if (ZipList.SelectedIndex > -1)
            {
                string state = stateList.SelectedItem.ToString();
                string city = cityList.SelectedItem.ToString();
                string zip = ZipList.SelectedItem.ToString();
                string qStr = "";
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        qStr = "Select * from (";
                        qStr += "SELECT  * FROM Business as B where " +
                               "B.state_name ='" + state +
                               "' AND B.city ='" + city + "' AND B.postal_code = '" + zip + "'";
                        qStr += ") as b inner join hours as h on b.business_id = h.business_id where";
                        qStr += " h.day_of_week ='" + dayList.SelectedItem.ToString() + "' AND (( h.start_hour <='" +
                                hours_s.SelectedItem.ToString() + "' AND h.end_hour >='" + hours_e.SelectedItem.ToString() + "') or (h.start_hour = '00:00' AND h.end_hour ='00:00'))";

                        string orderby = "";
                        if (sortBy.SelectedIndex > -1)
                        {
                            orderby = sortBy.SelectedItem.ToString();
                        }

                        switch (orderby)
                        {
                            default:
                                qStr += " order by name";
                                break;
                            case "Highest Rating":
                                qStr += " order by stars desc";
                                break;
                            case "Most Reviewed":
                                qStr += " order by review_count desc";
                                break;
                            case "Best Review Rating":
                                qStr += " order by review_rating desc";
                                break;
                            case "Most Checkins":
                                qStr += " order by numcheckins desc";
                                break;
                                //TODO: add nearest filter
                        }

                        cmd.CommandText = qStr;
                        double user_latitude = 0.0, user_longitude = 0.0;
                        if (latitudeBox.Text != "" && longitudeBox.Text != "")
                        {
                            user_latitude = Convert.ToDouble(latitudeBox.Text);
                            user_longitude = Convert.ToDouble(longitudeBox.Text);
                        }

                        using (var reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                double distanceAway = 0.0;
                                double businessLatitude = reader.GetDouble(9);
                                double businessLongitude = reader.GetDouble(10);
                                if (latitudeBox.Text != "" && longitudeBox.Text != "")
                                {
                                    var businessCoord = new GeoCoordinate(businessLatitude, businessLongitude);
                                    var userCoord = new GeoCoordinate(user_latitude, user_longitude);
                                    distanceAway = businessCoord.GetDistanceTo(userCoord) * 0.000621371;
                                }

                                businessGrid.Items.Add(new Business()
                                {
                                    bid = reader.GetString(0),
                                    name = reader.GetString(1),
                                    address = reader.GetString(3),
                                    state = reader.GetString(5),
                                    city = reader.GetString(6),
                                    //distance
                                    distance = distanceAway,
                                    //stars
                                    stars = reader.GetInt32(11),
                                    //numreviews
                                    numReviews = reader.GetInt32(12),
                                    //average rating of reviews
                                    avg_rating_review = reader.GetDouble(2),
                                    //total checkins
                                    numCheckins = reader.GetInt32(4)

                                });
                            }
                            if (orderby == "Nearest")
                            {
                                //businessGrid.Items.SortDescriptions.Add(new SortDescription("Distance", ListSortDirection.Ascending));
                                //businessGrid.Items.Refresh();
                                var column = businessGrid.Columns[4];

                                // Clear current sort descriptions
                                businessGrid.Items.SortDescriptions.Clear();

                                // Add the new sort description
                                businessGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, ListSortDirection.Ascending));

                                // Apply sort
                                foreach (var col in dataGrid.Columns)
                                {
                                    col.SortDirection = null;
                                }
                                column.SortDirection = ListSortDirection.Ascending;

                                // Refresh items to display sort
                                dataGrid.Items.Refresh();
                            }
                        }
                    }
                }

            }
        }
        private void filterByHours()
        {
            businessGrid.Items.Clear();
            if (ZipList.SelectedIndex > -1)
            {
                string state = stateList.SelectedItem.ToString();
                string city = cityList.SelectedItem.ToString();
                string zip = ZipList.SelectedItem.ToString();
                string qStr = "";

                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        qStr = "Select * from (";
                        qStr += "SELECT  * FROM Business where " +
                               "business.business_id in " +
                               "(select distinct temp1.business_id from" +
                               " (SELECT DISTINCT B.name, B.business_id FROM Business B, Category C " +
                               "WHERE B.business_id = C.business_id " +
                               "AND B.state_name='" + state + "' AND B.city ='" + cityList.SelectedItem.ToString() + "' AND B.postal_code = '" +
                               zip + "' AND C.category= '" + lvSelectedctgry.Items[0] + "') AS temp1 ";

                        for (int i = 1; i < lvSelectedctgry.Items.Count; i++)
                        {
                            qStr += "INNER JOIN (SELECT DISTINCT B.name, B.business_id" +
                                    " FROM Business B, Category C " +
                                    "WHERE B.business_id = C.business_id " +
                                    "AND B.state_name ='" + state +
                                    "' AND B.city ='" + city + "' AND B.postal_code = '" + zip +
                                    "' AND C.category = '" + lvSelectedctgry.Items[i] + "') AS temp" +
                                    (i + 1).ToString() + " ";
                        }

                        if (lvSelectedctgry.Items.Count > 1)
                        {
                            qStr += "ON ";

                            for (int i = 0; i < lvSelectedctgry.Items.Count - 1; i++)
                            {
                                qStr += "temp" + (i + 1).ToString() + ".name=" + "temp" + (i + 2).ToString() + ".name ";
                                if (i == lvSelectedctgry.Items.Count - 2)
                                {
                                    qStr += "";
                                }
                                else qStr += "AND ";
                            }
                        }

                        qStr += ")) as b inner join hours as h on b.business_id = h.business_id where";
                        qStr += " h.day_of_week ='" + dayList.SelectedItem.ToString() + "' AND (( h.start_hour <='" +
                                hours_s.SelectedItem.ToString() + "' AND h.end_hour >='" + hours_e.SelectedItem.ToString()+ "') or (h.start_hour = '00:00' AND h.end_hour ='00:00'))";
                        string orderby = "";
                        if (sortBy.SelectedIndex > -1)
                        {
                            orderby = sortBy.SelectedItem.ToString();
                        }
                        switch (orderby)
                        {
                            default:
                                qStr += " order by name";
                                break;
                            case "Highest Rating":
                                qStr += " order by stars desc";
                                break;
                            case "Most Reviewed":
                                qStr += " order by review_count desc";
                                break;
                            case "Best Review Rating":
                                qStr += " order by review_rating desc";
                                break;
                            case "Most Checkins":
                                qStr += " order by numcheckins desc";
                                break;
                            //TODO: add nearest filter
                        }


                        cmd.CommandText = qStr;
                        double user_latitude = 0.0, user_longitude = 0.0;
                        if (latitudeBox.Text != "" && longitudeBox.Text != "")
                        {
                            user_latitude = Convert.ToDouble(latitudeBox.Text);
                            user_longitude = Convert.ToDouble(longitudeBox.Text);
                        }
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                double distanceAway = 0.0;
                                double businessLatitude = reader.GetDouble(9);
                                double businessLongitude = reader.GetDouble(10);
                                if (latitudeBox.Text != "" && longitudeBox.Text != "")
                                {
                                    var businessCoord = new GeoCoordinate(businessLatitude, businessLongitude);
                                    var userCoord = new GeoCoordinate(user_latitude, user_longitude);
                                    distanceAway = businessCoord.GetDistanceTo(userCoord) * 0.000621371;
                                }
                                businessGrid.Items.Add(new Business()
                                {
                                    bid = reader.GetString(0),
                                    name = reader.GetString(1),
                                    address = reader.GetString(3),
                                    state = reader.GetString(5),
                                    city = reader.GetString(6),
                                    //distance
                                    distance =distanceAway,
                                    //stars
                                    stars = reader.GetInt32(11),
                                    //numreviews
                                    numReviews = reader.GetInt32(12),
                                    //average rating of reviews
                                    avg_rating_review = reader.GetDouble(2),
                                    //total checkins
                                    numCheckins = reader.GetInt32(4)

                                });
                            }
                            if (orderby == "Nearest")
                            {
                                //businessGrid.Items.SortDescriptions.Add(new SortDescription("Distance", ListSortDirection.Ascending));
                                //businessGrid.Items.Refresh();
                                var column = businessGrid.Columns[4];

                                // Clear current sort descriptions
                                businessGrid.Items.SortDescriptions.Clear();

                                // Add the new sort description
                                businessGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, ListSortDirection.Ascending));

                                // Apply sort
                                foreach (var col in dataGrid.Columns)
                                {
                                    col.SortDirection = null;
                                }
                                column.SortDirection = ListSortDirection.Ascending;

                                // Refresh items to display sort
                                dataGrid.Items.Refresh();
                            }
                        }
                    }
                }

            }
        }

        public void addColumnsToReviewGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = " Date";
            col1.Binding = new Binding("date");
            col1.Width = 100;
            reviewGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Name";
            col2.Binding = new Binding("userName");
            col2.Width = 100;
            reviewGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "Review";
            col3.Binding = new Binding("text");
            col3.Width = 400;
            reviewGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = " Stars";
            col4.Binding = new Binding("stars");
            col4.Width = 50;
            reviewGrid.Columns.Add(col4);
            
        }
        public void addColumnsToGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = " Business Name";
            col1.Binding = new Binding("name");
            col1.Width = 100;
            businessGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Address";
            col2.Binding = new Binding("address");
            col2.Width = 100;
            businessGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "City";
            col3.Binding = new Binding("city");
            col3.Width = 100;
            businessGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = " State";
            col4.Binding = new Binding("state");
            col4.Width = 50;
            businessGrid.Columns.Add(col4);

            DataGridTextColumn col5= new DataGridTextColumn();
            col5.Header = " Distance (In miles)";
            col5.Binding = new Binding("distance");
            col5.Width = 50;
            businessGrid.Columns.Add(col5);

            DataGridTextColumn col6= new DataGridTextColumn();
            col6.Header = " Stars";
            col6.Binding = new Binding("stars");
            col6.Width = 50;
            businessGrid.Columns.Add(col6);

            DataGridTextColumn col7 = new DataGridTextColumn();
            col7.Header = " # of reviews";
            col7.Binding = new Binding("numReviews");
            col7.Width = 50;
            businessGrid.Columns.Add(col7);

            DataGridTextColumn col8 = new DataGridTextColumn();
            col8.Header = " Avg rating review";
            col8.Binding = new Binding("avg_rating_review");
            col8.Width = 50;
            businessGrid.Columns.Add(col8);

            DataGridTextColumn col9 = new DataGridTextColumn();
            col9.Header = " Total checkins";
            col9.Binding = new Binding("numCheckins");
            col9.Width = 50;
            businessGrid.Columns.Add(col9);

            DataGridTextColumn col10 = new DataGridTextColumn();
            col10.Header = " Business_id";
            col10.Binding = new Binding("bid");
            col10.Width = 50;
            businessGrid.Columns.Add(col10);
            businessGrid.Columns[9].Visibility = Visibility.Collapsed;

        }

        public void addColumnsToFriendTipsGrid()
        {
            DataGridTextColumn userName = new DataGridTextColumn();
            userName.Header = "User Name";
            userName.Binding = new Binding("userName");
            userName.Width = 100;
            friendTipsGrid.Columns.Add(userName);

            DataGridTextColumn business = new DataGridTextColumn();
            business.Header = "Business";
            business.Binding = new Binding("business");
            business.Width = 200;
            friendTipsGrid.Columns.Add(business);

            DataGridTextColumn city = new DataGridTextColumn();
            city.Header = "City";
            city.Binding = new Binding("city");
            city.Width = 100;
            friendTipsGrid.Columns.Add(city);

            DataGridTextColumn distance = new DataGridTextColumn();
            distance.Header = "Distance Away (Miles)";
            distance.Binding = new Binding("distance");
            distance.Width = 150;
            friendTipsGrid.Columns.Add(distance);

            DataGridTextColumn text = new DataGridTextColumn();
            text.Header = "Text";
            text.Binding = new Binding("text");
            text.Width = 5000;
            friendTipsGrid.Columns.Add(text);
        }

        public void addColumnsToFriendDataGrid()
        {
            DataGridTextColumn name = new DataGridTextColumn();
            name.Header = "Name";
            name.Binding = new Binding("name");
            name.Width = 90;
            dataGrid.Columns.Add(name);

            DataGridTextColumn avgStars = new DataGridTextColumn();
            avgStars.Header = "Avg Stars";
            avgStars.Binding = new Binding("avgStars");
            avgStars.Width = 60;
            dataGrid.Columns.Add(avgStars);

            DataGridTextColumn yelpingSince = new DataGridTextColumn();
            yelpingSince.Header = "Yelping Since";
            yelpingSince.Binding = new Binding("yelpingSince");
            yelpingSince.Width = 80;
            dataGrid.Columns.Add(yelpingSince);
        }

        private void ZipList_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            businessGrid.Items.Clear();

            if (ZipList.SelectedIndex > -1)
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT name FROM business where postal_code = '" +
                                          ZipList.SelectedItem + "';";
                       //"SELECT name,city,state FROM business WHERE city = '" + cityList.SelectedItem.ToString() + "'AND state = '" + stateList.SelectedItem.ToString() + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                           /* while (reader.Read())
                            {

                                businessGrid.Items.Add(new Business()
                                {
                                    name = reader.GetString(0),
                                    
                                });
                            }*/
                            addCategories(ZipList.SelectedItem.ToString());
                        }
                    }

                    conn.Close();
                }
            }
        }

        private void addCategories( string zip)
        {
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select distinct category from business, category where postal_code = '" + zip +
                                      "' and business.business_id = category.business_id";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categoryList.Items.Add(reader.GetString(0));
                        }
                    }
                }

                conn.Close();
            }
        }

        //private void categoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    businessGrid.Items.Clear();
        //    if ((categoryList.SelectedIndex > -1) && (ZipList.SelectedIndex > -1))
        //    {
        //        using (var conn = new NpgsqlConnection(buildConnString()))
        //        {
        //            conn.Open();
        //            using (var cmd = new NpgsqlCommand())
        //            {
        //                cmd.Connection = conn;
        //                cmd.CommandText = "SELECT name, category FROM business, category where business.business_id" +
        //                                  " = category.business_id and category = '" + categoryList.SelectedItem +
        //                                  "'and postal_code ='"
        //                                  + ZipList.SelectedItem + "'; ";
        //                //"SELECT name,city,state FROM business WHERE city = '" + cityList.SelectedItem.ToString() + "'AND state = '" + stateList.SelectedItem.ToString() + "';";
        //                using (var reader = cmd.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {

        //                        businessGrid.Items.Add(new Business()
        //                        {
        //                            name = reader.GetString(0),
                                    
        //                        });
        //                    }
        //                }
        //            }

        //            conn.Close();
        //        }
        //    }
        //}

        private void button_Click(object sender, RoutedEventArgs e) //remove friend
        {
            string user_id = listBox.SelectedItem.ToString();
            if (dataGrid.SelectedItems[0] != null)
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        Friends row = (Friends)dataGrid.SelectedItems[0];
                        string friend_name = row.name;
                        cmd.Connection = conn;
                        cmd.CommandText = "delete from hasfriends where friend_id = (select user_id from usertable, (select friend_id from hasfriends where user_id = \'" + user_id + "\') as friends where usertable.user_id = friends.friend_id and usertable.name = \'" + friend_name + "\') and user_id = \'" + user_id + "\'";

                        cmd.ExecuteReader();
                    }
                    conn.Close();
                }
                updateFriendsandFriendsTips();
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            listBox.Items.Clear();
            friendTipsGrid.Items.Clear();
            //string name = textBox.Text;
            //if(name != "")
            //{
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "select user_id from usertable where name = \'" + textBox.Text + "\'";
                        try
                        {
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    listBox.Items.Add(reader.GetString(0));
                                }
                            }
                        }
                        catch(Exception exception)
                        {

                        }
                    }
                    conn.Close();
                    if(listBox.HasItems == false)
                    {
                        dataGrid.Items.Clear();
                        nameBox.Text = "";
                        starsBox.Text = "";
                        fansBox.Text = "";
                        yelpingSinceBox.Text = "";
                        funnyBox.Text = "";
                        coolBox.Text = "";
                        usefulBox.Text = "";
                    }
                }
            //}
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                string user_id = listBox.SelectedItem.ToString();
                string name;
                double stars;
                int fans;
                string yelping_since;
                int funny;
                int cool;
                int useful;
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "select * from usertable where user_id = \'" + user_id + "\'";
                        try
                        {
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    name = reader.GetString(1);
                                    stars = reader.GetDouble(2);
                                    fans = reader.GetInt32(3);
                                    yelping_since = reader.GetDate(5).ToString();
                                    funny = reader.GetInt32(7);
                                    cool = reader.GetInt32(8); 
                                    useful = reader.GetInt32(6);
                                    nameBox.Text = name;
                                    starsBox.Text = stars.ToString();
                                    fansBox.Text = fans.ToString();
                                    yelpingSinceBox.Text = yelping_since;
                                    funnyBox.Text = funny.ToString();
                                    coolBox.Text = cool.ToString();
                                    usefulBox.Text = useful.ToString();
                                }
                            }
                            cmd.CommandText = "select name, avg_stars, yelping_since from usertable, (select friend_id from hasfriends where user_id = \'" + user_id + "\') as friends where usertable.user_id = friends.friend_id";
                            using (var reader = cmd.ExecuteReader())
                            {
                                dataGrid.Items.Clear();
                                while (reader.Read())
                                {
                                    dataGrid.Items.Add(new Friends()
                                    {
                                        name = reader.GetString(0),
                                        avgStars = reader.GetDouble(1).ToString(),
                                        yelpingSince = reader.GetDate(2).ToString()
                                    });
                                }
                            }
                            cmd.CommandText = "select usertable.name, business.name, business.city, reviews.text, business.latitude, business.longitude from business, usertable, (select reviews1.user_id, reviews1.date_review, review.text, review.business_id from review, (select review.user_id, max(review.date_review) as date_review from review, (select friend_id from hasfriends where user_id = \'" + user_id + "\') as friends where review.user_id = friends.friend_id group by review.user_id) as reviews1 where reviews1.date_review = review.date_review and reviews1.user_id = review.user_id) as reviews where business.business_id = reviews.business_id and usertable.user_id = reviews.user_id";
                            double user_latitude = 0.0, user_longitude = 0.0;

                            if (latitudeBox.Text != "" && longitudeBox.Text != "")
                            {
                                user_latitude = Convert.ToDouble(latitudeBox.Text);
                                user_longitude = Convert.ToDouble(longitudeBox.Text);
                            }
                            using (var reader = cmd.ExecuteReader())
                            {
                                friendTipsGrid.Items.Clear();
                                while (reader.Read())
                                {
                                    double distanceAway = 0.0;
                                    double businessLatitude = reader.GetDouble(4);
                                    double businessLongitude = reader.GetDouble(5);
                                    if (latitudeBox.Text != "" && longitudeBox.Text != "")
                                    {
                                        var businessCoord = new GeoCoordinate(businessLatitude, businessLongitude);
                                        var userCoord = new GeoCoordinate(user_latitude, user_longitude);
                                        distanceAway = businessCoord.GetDistanceTo(userCoord)*0.000621371;
                                    }
                                    friendTipsGrid.Items.Add(new Review()
                                    {
                                        userName = reader.GetString(0),
                                        business = reader.GetString(1),
                                        city = reader.GetString(2),
                                        text = reader.GetString(3),
                                        distance = distanceAway
                                    });

                                    
                                        //businessGrid.Items.SortDescriptions.Add(new SortDescription("Distance", ListSortDirection.Ascending));
                                        //businessGrid.Items.Refresh();
                                        var column = friendTipsGrid.Columns[3];

                                        // Clear current sort descriptions
                                        friendTipsGrid.Items.SortDescriptions.Clear();

                                        // Add the new sort description
                                        friendTipsGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, ListSortDirection.Ascending));

                                        // Apply sort
                                        foreach (var col in dataGrid.Columns)
                                        {
                                            col.SortDirection = null;
                                        }
                                        column.SortDirection = ListSortDirection.Ascending;

                                        // Refresh items to display sort
                                        friendTipsGrid.Items.Refresh();
                                    
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception.ToString());
                        }
                    }
                    conn.Close();
                }
            }
        }

        private void updateFriendsandFriendsTips()
        {
            string user_id = listBox.SelectedItem.ToString();
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    try
                    {
                        cmd.CommandText = "select name, avg_stars, yelping_since from usertable, (select friend_id from hasfriends where user_id = \'" + user_id + "\') as friends where usertable.user_id = friends.friend_id";
                        using (var reader = cmd.ExecuteReader())
                        {
                            dataGrid.Items.Clear();
                            while (reader.Read())
                            {
                                dataGrid.Items.Add(new Friends()
                                {
                                    name = reader.GetString(0),
                                    avgStars = reader.GetDouble(1).ToString(),
                                    yelpingSince = reader.GetDate(2).ToString()
                                });
                            }
                        }
                        cmd.CommandText = "select usertable.name, business.name, business.city, reviews.text, business.latitude, business.longitude from business, usertable, (select reviews1.user_id, reviews1.date_review, review.text, review.business_id from review, (select review.user_id, max(review.date_review) as date_review from review, (select friend_id from hasfriends where user_id = \'" + user_id + "\') as friends where review.user_id = friends.friend_id group by review.user_id) as reviews1 where reviews1.date_review = review.date_review and reviews1.user_id = review.user_id) as reviews where business.business_id = reviews.business_id and usertable.user_id = reviews.user_id";
                        double user_latitude = 0.0, user_longitude = 0.0;

                        if (latitudeBox.Text != "" && longitudeBox.Text != "")
                        {
                            user_latitude = Convert.ToDouble(latitudeBox.Text);
                            user_longitude = Convert.ToDouble(longitudeBox.Text);
                        }
                        using (var reader = cmd.ExecuteReader())
                        {
                            friendTipsGrid.Items.Clear();
                            while (reader.Read())
                            {
                                double distanceAway = 0.0;
                                double businessLatitude = reader.GetDouble(4);
                                double businessLongitude = reader.GetDouble(5);
                                if (latitudeBox.Text != "" && longitudeBox.Text != "")
                                {
                                    var businessCoord = new GeoCoordinate(businessLatitude, businessLongitude);
                                    var userCoord = new GeoCoordinate(user_latitude, user_longitude);
                                    distanceAway = businessCoord.GetDistanceTo(userCoord) * 0.000621371;
                                }
                                friendTipsGrid.Items.Add(new Review()
                                {
                                    userName = reader.GetString(0),
                                    business = reader.GetString(1),
                                    city = reader.GetString(2),
                                    text = reader.GetString(3),
                                    distance = distanceAway
                                });

                                //businessGrid.Items.SortDescriptions.Add(new SortDescription("Distance", ListSortDirection.Ascending));
                                //businessGrid.Items.Refresh();
                                var column = friendTipsGrid.Columns[3];

                                // Clear current sort descriptions
                                friendTipsGrid.Items.SortDescriptions.Clear();

                                // Add the new sort description
                                friendTipsGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, ListSortDirection.Ascending));

                                // Apply sort
                                foreach (var col in dataGrid.Columns)
                                {
                                    col.SortDirection = null;
                                }
                                column.SortDirection = ListSortDirection.Ascending;

                                // Refresh items to display sort
                                friendTipsGrid.Items.Refresh();
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.ToString());
                    }
                }
                conn.Close();
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e) //set location
        {
            if (latitudeBox.Text != "" && longitudeBox.Text != "")
            {
                double user_latitude = Convert.ToDouble(latitudeBox.Text);
                double user_longitude = Convert.ToDouble(longitudeBox.Text);

                string user_id = listBox.SelectedItem.ToString();
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        try
                        {
                            cmd.CommandText = "select usertable.name, business.name, business.city, reviews.text, business.latitude, business.longitude from business, usertable, (select reviews1.user_id, reviews1.date_review, review.text, review.business_id from review, (select review.user_id, max(review.date_review) as date_review from review, (select friend_id from hasfriends where user_id = \'" + user_id + "\') as friends where review.user_id = friends.friend_id group by review.user_id) as reviews1 where reviews1.date_review = review.date_review and reviews1.user_id = review.user_id) as reviews where business.business_id = reviews.business_id and usertable.user_id = reviews.user_id";
                            using (var reader = cmd.ExecuteReader())
                            {
                                friendTipsGrid.Items.Clear();
                                while (reader.Read())
                                {
                                    double distanceAway = 0.0;
                                    double businessLatitude = reader.GetDouble(4);
                                    double businessLongitude = reader.GetDouble(5);
                                    var businessCoord = new GeoCoordinate(businessLatitude, businessLongitude);
                                    var userCoord = new GeoCoordinate(user_latitude, user_longitude);
                                    distanceAway = businessCoord.GetDistanceTo(userCoord) * 0.000621371;
                                    friendTipsGrid.Items.Add(new Review()
                                    {
                                        userName = reader.GetString(0),
                                        business = reader.GetString(1),
                                        city = reader.GetString(2),
                                        text = reader.GetString(3),
                                        distance = distanceAway
                                    });

                                    //businessGrid.Items.SortDescriptions.Add(new SortDescription("Distance", ListSortDirection.Ascending));
                                    //businessGrid.Items.Refresh();
                                    var column = friendTipsGrid.Columns[3];

                                    // Clear current sort descriptions
                                    friendTipsGrid.Items.SortDescriptions.Clear();

                                    // Add the new sort description
                                    friendTipsGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, ListSortDirection.Ascending));

                                    // Apply sort
                                    foreach (var col in dataGrid.Columns)
                                    {
                                        col.SortDirection = null;
                                    }
                                    column.SortDirection = ListSortDirection.Ascending;

                                    // Refresh items to display sort
                                    friendTipsGrid.Items.Refresh();
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception.ToString());
                        }
                    }
                    conn.Close();
                }
            }
        }

     

        

        private void buttonRmvctgry_Click(object sender, RoutedEventArgs e)
        {
            if (lvSelectedctgry.SelectedIndex > -1)
            {
                lvSelectedctgry.Items.Remove(lvSelectedctgry.SelectedItem);
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (categoryList.SelectedIndex > -1)
            {
                lvSelectedctgry.Items.Add(categoryList.SelectedItem);
            }
        }

        private void dayList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((dayList.SelectedIndex > -1) && (hours_e.SelectedIndex > -1) && (hours_s.SelectedIndex > -1))
            {
                //filterByHours();
                if (lvSelectedctgry.Items.Count > 0)
                {
                    filterByHours();
                }
                else
                {
                    filterByHoursNoCategory();
                }
            }
        }

        private void hours_s_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((dayList.SelectedIndex > -1) && (hours_e.SelectedIndex > -1) && (hours_s.SelectedIndex > -1))
            {
                //filterByHours();
                if (lvSelectedctgry.Items.Count > 0)
                {
                    filterByHours();
                }
                else
                {
                    filterByHoursNoCategory();
                }
            }
        }

        private void hours_e_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((dayList.SelectedIndex > -1) && (hours_e.SelectedIndex > -1) && (hours_s.SelectedIndex > -1))
            {
                //filterByHours();
                if (lvSelectedctgry.Items.Count > 0)
                {
                    filterByHours();
                }
                else
                {
                    filterByHoursNoCategory();
                }
            }
        }

        private void sortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((dayList.SelectedIndex > -1) && (hours_e.SelectedIndex > -1) && (hours_s.SelectedIndex > -1))
            {
                //filterByHours();
                if (lvSelectedctgry.Items.Count > 0)
                {
                    filterByHours();
                }
                else
                {
                    filterByHoursNoCategory();
                }
            }
            else
            {
                //search();
                if (lvSelectedctgry.Items.Count > 0)
                {
                    search();
                }
                else
                {
                    searchNoCategories();
                }
            }
        }

        private void businessGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (businessGrid.SelectedIndex > -1)
            {
                Business b = (Business)businessGrid.SelectedItem;
                selectedBusinessName.Text = b.name;
            }
        }

        private void btCheckin_Click(object sender, RoutedEventArgs e)
        {
            if ((selectedBusinessName.Text != "")) // we have a business
            {
                Business b = (Business) businessGrid.SelectedItem;
                var src = DateTime.Now;
                int hour = src.Hour;
                string day = src.DayOfWeek.ToString();
                string qStr = "update checkin set ";
                if((hour >=6)&&(hour <12))
                {
                    qStr += "morning = morning + 1";
                }
                else if ((hour >= 12) && (hour < 5))
                {
                    qStr += "afternoon = afternoon + 1";
                }
                else if ((hour >= 5) && (hour < 23))
                {
                    qStr += "evening = evening + 1";
                }
                else
                {
                    qStr += "night = night + 1";
                }

                qStr += " where business_id='" + b.bid + "' AND day_of_week = '";
                qStr += day + "'";

                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = qStr;
                        cmd.ExecuteNonQuery();
                    }

                    if ((dayList.SelectedIndex > -1) && (hours_e.SelectedIndex > -1) && (hours_s.SelectedIndex > -1))
                    {
                        //filterByHours();
                        if (lvSelectedctgry.Items.Count > 0)
                        {
                            filterByHours();
                        }
                        else
                        {
                            filterByHoursNoCategory();
                        }
                    }
                    else
                    {
                        //search();
                        if (lvSelectedctgry.Items.Count > 0)
                        {
                            search();
                        }
                        else
                        {
                            searchNoCategories();
                        }
                    }

                    conn.Close();
                }

            }
        }
        
        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private void btAddReview_Click(object sender, RoutedEventArgs e)
        {
            if ((newReviewText.Text != "") && (AddRatingBox.SelectedIndex > -1) && (listBox.SelectedIndex > -1)&&(businessGrid.SelectedIndex > -1))
            {
                //genrate random review_id
                string rid = RandomString(22);
                var src = DateTime.Now;
                var date = src.Date;
                Business b = (Business)businessGrid.SelectedItem;

                string temp = newReviewText.Text;
                temp = temp.Replace(",", string.Empty);
                temp = temp.Replace("\'", string.Empty);
                temp= temp.Replace("\n", " ");
                temp = temp.Replace("\r", " ");
                string qStr = "insert into Review(review_id,business_id,user_id,stars,date_review,text) Values ('";
                qStr += rid + "','" + b.bid + "','" + listBox.SelectedItem.ToString() + "'," +
                        AddRatingBox.SelectedItem.ToString() + ",'" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + temp + "')";
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = qStr;
                        cmd.ExecuteNonQuery();
                    }

                    if ((dayList.SelectedIndex > -1) && (hours_e.SelectedIndex > -1) && (hours_s.SelectedIndex > -1))
                    {
                        //filterByHours();
                        if (lvSelectedctgry.Items.Count > 0)
                        {
                            filterByHours();
                        }
                        else
                        {
                            filterByHoursNoCategory();
                        }
                    }
                    else
                    {
                        //search();
                        if (lvSelectedctgry.Items.Count > 0)
                        {
                            search();
                        }
                        else
                        {
                            searchNoCategories();
                        }
                    }

                    conn.Close();
                }
            }
        }

        private void btBusPerZip_Click(object sender, RoutedEventArgs e)
        {
            if ((stateList.SelectedIndex > -1) && (cityList.SelectedIndex > -1))
            {
                generateGraphZip();
            }
        }

        private void generateGraphZip()
        {
            List<KeyValuePair<string, int>> chartData = new List<KeyValuePair<string, int>>();
            string state = stateList.SelectedItem.ToString();
            string city = cityList.SelectedItem.ToString();

            string qStr = "select postal_code,count(business_id) from business " +
                          "where state_name = '" + state + "' and city ='" + city + "' group by " +
                          "postal_code order by postal_code";
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = qStr;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            chartData.Add(new KeyValuePair<string,int>(reader.GetString(0),reader.GetInt32(1)));
                        }
                    }
                }

                conn.Close();
            }

            Aggregations.Title = "# Businesses";
            Aggregations.DataContext = chartData;
        }

        private void btShowCheckins_Click(object sender, RoutedEventArgs e)
        {
            if (businessGrid.SelectedIndex > -1)
            {
                if ((checkinDayChart.SelectedIndex > -1)&& (checkinDayChart.SelectedItem.ToString() != "Default"))
                {
                    generateGraphCheckinTime();
                }                

                else
                {
                    generateGraphCheckin();
                }
            }
            
        }

        private void generateGraphCheckinTime()
        {
            List<KeyValuePair<string, int>> chartData = new List<KeyValuePair<string, int>>();
            Business b = (Business)businessGrid.SelectedItem;

            string qStr = "select morning,afternoon, evening, night from checkin where business_id = '" + b.bid;
            qStr += "' and day_of_week = '" + checkinDayChart.SelectedItem.ToString() + "'";

            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = qStr;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            chartData.Add(new KeyValuePair<string, int>("morning", reader.GetInt32(0)));
                            chartData.Add(new KeyValuePair<string, int>("afternoon", reader.GetInt32(1)));
                            chartData.Add(new KeyValuePair<string, int>("evening", reader.GetInt32(2)));
                            chartData.Add(new KeyValuePair<string, int>("night", reader.GetInt32(3)));
                        }
                    }
                }

                conn.Close();
            }

            Aggregations.Title = "Checkins for time of day";
            Aggregations.DataContext = chartData;

        }
        private void generateGraphCheckin()
        {
            List<KeyValuePair<string, int>> chartData = new List<KeyValuePair<string, int>>();
            Business b = (Business) businessGrid.SelectedItem;


            string qStr = "select day_of_week,sum(afternoon) + sum(night) + " +
                          "sum(morning) + sum(evening) as total from checkin where business_id = '" + b.bid +
                          "'group by day_of_week";
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = qStr;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            chartData.Add(new KeyValuePair<string, int>(reader.GetString(0), reader.GetInt32(1)));
                        }
                    }
                }

                conn.Close();
            }

            Aggregations.Title = "Checkins per day of week";
            Aggregations.DataContext = chartData;
        }

        private void btShowReviews_Click(object sender, RoutedEventArgs e)
        {
            if (businessGrid.SelectedIndex > -1)
            {
                showReviews();
            }
        }

        private void showReviews()
        {
            Business b = (Business)businessGrid.SelectedItem;

            /*
            public string date { get; set; }
            public string userName { get; set; }
            public int stars{ get; set; }
            public string text { get; set; }
            public bool funny{ get; set; }
            public bool useful { get; set; }
            public bool cool{ get; set; }
             */
            string qStr = "select date_review,name, stars, text from usertable, (select * from review where business_id = '" +
                           b.bid + "') as reviews where usertable.user_id = reviews.user_id";
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = qStr;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            
                            
                            reviewGrid.Items.Add(new Review_Display()
                            {
                                date = reader.GetDate(0).ToString(),
                                userName = reader.GetString(1),
                                stars = reader.GetInt32(2),
                                text = reader.GetString(3),
                            });
                        }
                    }
                }

                conn.Close();
            }
        }

        private void checkinDayChart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (businessGrid.SelectedIndex > -1)
            {
                if ((checkinDayChart.SelectedIndex > -1) || (checkinDayChart.SelectedItem.ToString() != "Default"))
                {
                    generateGraphCheckinTime();
                }
                else
                {
                    generateGraphCheckin();
                }
            }
        }
    }
}
