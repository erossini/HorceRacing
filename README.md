# HorseRacing Simulator

This solution is a console application that simulates and displays the result of a horserace.
It consists of three projects:

* [HorseRacing.Domain](https://github.com/erossini/HorseRacing/tree/master/HorseRacing.Domain) 
* [HorseRacing.Domain.Tests](https://github.com/erossini/HorseRacing/tree/master/HorseRacing.Tests)
* [HorseRacing.Console](https://github.com/erossini/HorseRacing/tree/master/HorseRacing.Console)

All projects is written in C# and target the .Net Core framework. A reasonable amount of effort has been put so that this solution demonstrates:
* Dependency Injection (using Autofac)
* Logging (using Serilog)
* Task Parallel Library
* Test Driven Design
* Domain Driven Design

# HorseRacing.Domain
This project implements the object model of the domain that incorporates both behavior and data.
The following classes implemented:
* [Horse](https://github.com/erossini/HorseRacing/blob/master/HorseRacing.Domain/Entities/Horse.cs)
  Horse class is a wrapper around a horse name. It is a value type and it is immutable. The name is as per the British Horseracing Authority rules, a maximum of 18 characters long (A-Z only including spaces).
* [OddsPrice](https://github.com/erossini/HorseRacing/blob/master/HorseRacing.Domain/Entities/OddsPrice.cs)
  OddsPrice class is a wrapper around the Odds price. It is a value type and it is immutable.Be able to be given a valid UK fractional odds price. This is in the string format of “x/y” where x and y are integer numbers greater than 0 e.g 2/1, 10/1, 5/2
* [Runner](https://github.com/erossini/HorseRacing/blob/master/HorseRacing.Domain/Entities/Runner.cs) 
  Runner class is an identifiable entity, combining a horse and an odds price. A runner can participate in a Horse Race.
* [HorseRace](https://github.com/erossini/HorseRacing/blob/master/HorseRacing.Domain/Entities/HorseRace.cs)
  HorseRace consists of a list of runners as well as state information and the winner calculation.


# HorseRacing.Domain.Tests
  This project consists of all the tests. xUnit framework has been used as well as FluentAssertions. the test named        [Check_that_winner_frequency_is_as_expected()](https://github.com/erossini/HorseRacing/blob/master/HorseRacing.Tests/HorseRaceSpecs.cs) verifies that for a given race, over the course of 1,000,000 iterations of calculating the winner, the results are within 2% either way for each runner.


# HorseRacing.Console

This project is a console application that holds the user interface for the simulator.
The user interface consists of the following areas
* The Title
The application title is "Horse Race Simulator"
* The race summary
  The following information is shown
  * The number of runners in the race.
  * The current race margin.
  * The current race state (i.e. Empty, Full).
  * The total number of runs.
  * The last race winner.
* The runners
  The following information is shown.
  * The runner name and slot.
  * The odds price in the string format of x/y.
  * The current runner margin.
  * The current runner chance of winning the race.
  * The number of races the current runner won.
  * The number of races the current runner won as percentage of total races.

  The top runner is marked with an asterisk.
* The menu
A detailed explaination of the menu options will be discussed in the next section.
* The message area
Any messages will be displayed in this area.

![alt](https://github.com/erossini/HorseRacing/blob/master/screenshot.jpg)

The menu contains the following options:

* A - Adds a new runner i.e. horse and odds to the race. This option is available only if there are less than 16 runners in the race.
* D - Adds a predefined list of 16 runners. This option is available only if there are no runners in the race.
* C - Clears the runners and reinitialises the state data. This option is available only if there is at least one runner in the race.
* R - Calculates the winner of the race (single iteration). This option is available only if there are 4 or more runners in the race, and the margin is between 110 and 140
* B - Calculates the winner of the race (multiple iterations).This option is available only if there are 4 or more runners in the race, and the margin is between 110 and 140
* S - Cancels the running simulation. This option is only available if there is a running simulation.

The options are displayed in darkgray color when they are disabled.
