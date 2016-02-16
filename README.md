# SimpleStock
The application under Visual Studio and C# that gets basic stock info from a list of NASDAQ companies.

Major parts of the appliction:
-Gets the list of NASDAQ companies from a text file, removes everything except what is needed
-Adds the adjusted list to a list in C# and displays the list on the GUI.
-Has a search bar (that doesn't work at the moment, but the idea is to link the user's input to whats displayed in the list)
-Once the Analyze button is pressed, the application retrieves the data from Google finance/ Yahoo finance and displays it.

Needed changes:
-Add a autocomplete function to the search
-Add more financial information.
-Add a custom calculation that clumps the displayed into one possible value
