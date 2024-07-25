using System.Text;

class Program {
	static void Main(string[] args) {
		List<String> inputList = AskUserInput();

		var formattedInput = ConvertInputToAlgorithmFormat(inputList);

		Tuple<string, string, int> result = Algorithm(formattedInput);

		Console.WriteLine(
				$"\nThe busiest time is {result.Item1}-{result.Item2} with total of {result.Item3} driver(s) taking a break");
	}

	static List<List<int>> ConvertInputToAlgorithmFormat(List<String> timesAsStrings) {
		var formattedList = new List<List<int>>();

		foreach (var i in timesAsStrings) {
			List<String> splitTime = SplitStringEveryNCharacters(i, 5);
			var subList = new List<int>();
			subList.Add(ConvertTimeToInt(splitTime[0]));
			subList.Add(ConvertTimeToInt(splitTime[1]));
			formattedList.Add(subList);
		}

		return formattedList;
	}
	static List<string> SplitStringEveryNCharacters(string str, int num) {
		List<string> segments = new List<string>();

		for (int i = 0; i < str.Length; i += num) {
			if (i + num <= str.Length) {
				segments.Add(str.Substring(i, num));
			} else {
				segments.Add(str.Substring(i));
			}
		}

		return segments;
	}
	static List<String> ReadInputFromConsole() {
		int drivers = 0;
		bool continueFunction = false;

		Console.WriteLine("\nHow many drivers are taking a break?");
		string input = Console.ReadLine() ?? "";

		// Check if user input is positive number
		while (!continueFunction) {
			if (int.TryParse(input, out drivers) && drivers > 0) {
				continueFunction = true;
			} else {
				Console.WriteLine("\nYou can only write positive numbers");
				Console.WriteLine("\nHow many drivers are taking a break?");
				input = Console.ReadLine() ?? "";
			}
		}

		List<String> times = [];

		// Check if user inputs valid time in correct format
		for (int i = 0; i < drivers; i++) {
			Console.WriteLine("\nEnter break time:");
			var timeInput = Console.ReadLine() ?? "";
			try {
				List<String> splitTimes = SplitStringEveryNCharacters(timeInput, 5);
				ConvertTimeToInt(splitTimes[0]);
				times.Add(timeInput);
			} catch (Exception) {
				Console.WriteLine("This time isn't valid!");
				i--;
			}
		}
		return times;
	}

	static bool IsValidPath(string path) {
		// Check if the path is null or empty
		if (string.IsNullOrWhiteSpace(path)) {
			return false;
		}

		try {
			// Check for invalid path characters
			string root = Path.GetPathRoot(path);
			if (string.IsNullOrWhiteSpace(root)) {
				return false;
			}

			// Check if the path contains invalid characters
			if (path.IndexOfAny(Path.GetInvalidPathChars()) >= 0) {
				return false;
			}

			// Optionally, check if the path exists
			if (!Directory.Exists(path) && !File.Exists(path)) {
				return false;
			}

			return true;
		} catch (Exception) {
			// If any exception occurs, the path is considered invalid
			return false;
		}
	}

	static List<String> ReadInputFromFile() {
		string fileName = "";
		bool isGoodPath = false;

		Console.WriteLine("Insert .txt file PATH");

		// Check if user inputs valid PATH
		while (!isGoodPath) {
			fileName = Console.ReadLine();

			if (IsValidPath(fileName)) {
				isGoodPath = true;
			} else {
				Console.WriteLine("This path is not valid!");
				Console.WriteLine("\nPlease enter new path");
			}
		}

		List<String> times = [];

		try {
			using(StreamReader reader = new StreamReader(fileName)) {
				string line;
				while ((line = reader.ReadLine()) != null) {
					times.Add(line);
				}
			}
		} catch (Exception e) {
			Console.WriteLine("An error occurred:");
			Console.WriteLine(e.Message);
		}
		return times;
	}
	static List<String> AskUserInput() {
		Console.WriteLine("Welcome to Break Calculator\n");
		Console.WriteLine("Time entry format <start-time><end-time> (i.e 11:1512:00)\n");
		Console.WriteLine("To continue press:");
		Console.WriteLine("1 - to input break times in Command Line");
		Console.WriteLine("or");
		Console.WriteLine("2 - to input break times from the Text File");

		int inputInteger = 0;
		bool continueFunction = false;
		string input = Console.ReadLine() ?? "";

		while (!continueFunction) {
			int.TryParse(input, out inputInteger);
			if (inputInteger == 1 || inputInteger == 2) {
				continueFunction = true;
			} else {
				Console.WriteLine("You can only choose 1 or 2");
				input = Console.ReadLine() ?? "";
			}
		}

		List<String> times = [];

		if (inputInteger == 1) {
			times = ReadInputFromConsole();
		} else {
			times = ReadInputFromFile();
		}

		return times;
	}
	static int ConvertTimeToInt(string time) {
		var items = time.Split(":");
		int hours;
		int minutes;

		Int32.TryParse(items[0], out hours);
		Int32.TryParse(items[1], out minutes);

		return hours * 60 + minutes;
	}

	static string ConvertIntToTime(int number) {
		int hours;
		int minutes;

		hours = number / 60;
		minutes = number - (hours * 60);

		return $"{hours.ToString()}:{minutes.ToString()}";
	}
	static Tuple<string, string, int> Algorithm(List<List<int>> times) {
		var events = new List<List<int>>();

		// Separate array into components to use in algorithm
		for (int i = 0; i < times.Count; i++) {
			var startInfo = new List<int>();
			startInfo.Add(times [i][0]);
			startInfo.Add(1);
			events.Add(startInfo);
			var endInfo = new List<int>();
			endInfo.Add(times [i][1]);
			endInfo.Add(0);
			events.Add(endInfo);
		}

		// Sort Events by time, if some event starts and ends at the same time, sort so the the ending would be first
		events.Sort((a, b) => {
			int compareFirst = a [0].CompareTo(b[0]);
			if (compareFirst != 0) {
				return compareFirst;
			}
			return a [1].CompareTo(b[1]);
		});

		int maxOverlap = 0;
		int currentOverlap = 0;
		int busiestStart = 0;
		int busiestEnd = 0;
		int currentStart = 0;

		foreach (var i in events) {
			if (i[1] == 1) {
				currentOverlap += 1;

				if (currentOverlap > maxOverlap) {
					maxOverlap = currentOverlap;
					busiestStart = i[0];
					currentStart = i[0];
				}
			} else {
				if (currentOverlap == maxOverlap) {
					busiestEnd = i[0];
				}
				currentOverlap -= 1;
			}
		}
		return Tuple.Create(ConvertIntToTime(busiestStart), ConvertIntToTime(busiestEnd), maxOverlap);
	}
}