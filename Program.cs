using System.Text;

class Program
{

  static List<string> SplitStringEveryNCharacters(string str, int number)
    {
        List<string> segments = new List<string>();

        for (int i = 0; i < str.Length; i += number)
        {
            if (i + number <= str.Length)
            {
                segments.Add(str.Substring(i, number));
            }
            else
            {
                segments.Add(str.Substring(i));
            }
        }

        return segments;
    }
  static List<String> ReadInputFromConsole()
  {
    Console.WriteLine("How many drivers are taking a break?");
    int drivers = 0;
    bool continueFunction = false;
    string input = Console.ReadLine();

    while(!continueFunction)
    {
      if(int.TryParse(input, out drivers))
      {
        continueFunction = true;
      }
      else 
      {
        Console.WriteLine("You can only write numbers");
        Console.WriteLine("How many drivers are taking a break?");
        input = Console.ReadLine();
      }
    }
    List<String> times = [];

    for (int i = 0; i < drivers; i++) 
    {
      times.Add(Console.ReadLine());
    }
    return times;
  }
  static List<String> ReadInputFromFile()
  {
    Console.WriteLine("Insert .txt file PATH");

    // How the f do I check if this shit is valid
    string fileName =  "";
    bool isGoodPath = false;

    while (!isGoodPath)
    {
      try 
      {
        fileName = Console.ReadLine();
        File.OpenRead(fileName);
        isGoodPath = true;
      } 
      catch(Exception)
      {
        Console.WriteLine("This path is not valid!");
        Console.WriteLine("Please enter new path");
      }
    }
    List<String> times = [];

    const Int32 BufferSize = 128;
    using (var fileStream = File.OpenRead(fileName))
      using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize)) {
        String line;
        while ((line = streamReader.ReadLine()) != null)
        {
          times.Add(line);
        }

    return times;
  }

  }
  static List<String> AskUserInputType()
  {
    Console.WriteLine("Welcome to Break Calculator");
    Console.WriteLine("To continue press:");
    Console.WriteLine("1 - to input break times in Command Line");
    Console.WriteLine("or");
    Console.WriteLine("2 - to input break times from the Text File");
    int X = 0;
    bool continueFunction = false;
    string input = Console.ReadLine();
    
    while(!continueFunction)
    {
      int.TryParse(input, out X);
      if (X == 1 || X == 2)
      {
        continueFunction = true;
      }
      else 
      {
        Console.WriteLine("You can only choose 1 or 2");
        input = Console.ReadLine();
      }
    }
    
    List<String> times = [];

    if (X == 1)
    {
      times = ReadInputFromConsole();
    } 
    else
    {
      times = ReadInputFromFile();
    }

    return times;
  }
  static int ConvertTimeToInt(string time) 
  {
    var items = time.Split(":");
    int hours = 0;
    int minutes = 0;

    Int32.TryParse(items[0], out hours);
    Int32.TryParse(items[1], out minutes);

    return hours*60+minutes;
  }

  static string ConvertIntToTime(int number)
  {
    int hours;
    int minutes;

    hours = number/60;
    minutes = number-(hours*60);

    return hours.ToString()+":"+minutes.ToString();
  }
  static Tuple<string, string, int> Algorithm(List<List<int>> times) 
  {
    var events = new List<List<int>>();

    // Separate array into components to use in algorithm
    for (int i = 0; i < times.Count; i++) {
        var start_info = new List<int>();
        start_info.Add(times[i][0]);
        start_info.Add(1);      
        events.Add(start_info);
        var end_info = new List<int>();
        end_info.Add(times[i][1]);
        end_info.Add(0);      
        events.Add(end_info);
    }

    // Sort Events by time, if some event starts and ends at the same time, sort so the the ending would be first
    events.Sort((a,b) => {
            int compareFirst = a[0].CompareTo(b[0]);
            if (compareFirst != 0)
            {
                return compareFirst;
            }
            return a[1].CompareTo(b[1]);
        });

    int max_overlap = 0;
    int current_overlap = 0;
    int busiest_start = 0;
    int busiest_end = 0;
    int current_start = 0;


    foreach(var i in events)
    {   
        if (i[1] == 1) {
          current_overlap += 1; 
          
          if (current_overlap > max_overlap) {
            max_overlap = current_overlap;
            busiest_start = i[0];
            current_start = i[0];
          }
        }
        else {
          if (current_overlap == max_overlap) {
            busiest_end = i[0];
          }
          current_overlap -= 1;
        }

    }
    return Tuple.Create(ConvertIntToTime(busiest_start), ConvertIntToTime(busiest_end), max_overlap);

  }

  static void Main(string[] args)
  {
    List<String> timesString =  AskUserInputType();

    var timesInt = new List<List<int>>();
    
  
    foreach(var i in timesString)
    {

      List<String> splitTime = SplitStringEveryNCharacters(i, 5);
      var subList = new List<int>();
      subList.Add(ConvertTimeToInt(splitTime[0]));
      subList.Add(ConvertTimeToInt(splitTime[1]));
      timesInt.Add(subList);
    }
    Tuple<string, string, int> output = Algorithm(timesInt);
    Console.WriteLine("The busiest time is " + output.Item1 + "-" + output.Item2 + " with total of " + output.Item3 + " drivers taking a break");
  }
}