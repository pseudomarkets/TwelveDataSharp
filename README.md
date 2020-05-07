# TwelveDataSharp
A .NET Standard 2.0 library for accessing Twelve Data's stock market APIs

https://twelvedata.com/docs

# Features
* Compatible with projects that target .NET Standard 2.0 including .NET Framework, .NET Core, and Xamarin
* Aims to access all APIs (work in progress, starting with Time Series and Reference Data first)
* Lightweight with with just a single external dependency (Newtonsoft.JSON)
* Response data from APIs is converted automatically and ready to use, including conversion of numerical data in strings to the appropriate primtive type

# Usage

Just create a client object with your Twelve Data API key and call the async function for the respective API endpoint you'd like to hit. Additional detailed documentation is coming soon.

`using TwelveDataSharp;`

`TwelveDataClient client = new TwelveDataClient("YOUR_API_KEY_HERE");`

`var response = await client.GetTimeSeriesAsync("AAPL");`

`Console.WriteLine("Open:" + response.Values[0].Open);`

# NuGet
A NuGet package will be available on nuget.org after additional endpoints are added and testing is performed, stay tuned. In the meantime you can always clone the repo and build the nuget package yourself.

# Notice
This is NOT an official Twelve Data library, and the author of this library is not affiliated with Twelve Data in any way, shape or form. Twelve Data APIs and data are Copyright © 2020 Twelve Data Pte. Ltd.
