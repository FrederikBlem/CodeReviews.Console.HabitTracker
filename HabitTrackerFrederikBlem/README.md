# Habit Tracker Project

A console based CRUD application project that I'm using to refresh my C# knowledge and skills from scratch courtesy of C# Academy.

You can find the full assignment at [csharpacademy.com](https://www.thecsharpacademy.com/project/12/habit-logger).

## Academy Requirements

+ [x] This is an application where you’ll log occurrences of a habit.
+ [x] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day).
+ [x] Users need to be able to input the date of the occurrence of the habit.
+ [x] The application should store and retrieve data from a real database.
+ [x] When the application starts, it should create a sqlite database, if one isn’t present.
+ [x] It should also create a table in the database, where the habit will be logged.
+ [x] The users should be able to insert, delete, update and view their logged habit.
+ [x] You should handle all possible errors so that the application never crashes.
+ [x] You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.
+ [x] Follow the DRY Principle, and avoid code repetition.
+ [x] Your project needs to contain a Read Me file where you'll:
    + [ ] explain how your app works.
    + [ ] tell a little bit about your thought progress.
    + [ ] What was hard?
    + [ ] What was easy?
    + [ ] What have you learned?

## How the app works
In order to make it easier to develop, the Habit Tracker assumes the current culture is "en-GB", this means that the app is in English and dates, decimal notation etc. follow Great Britain standards.

Upon starting the Habit Tracker application, it will create a database file "habittracker.db" if one doesn't already exist. This is where habit tables can be created, read, updated and deleted and habits belonging to those tables can likewise be created, read, updated and deleted.

The person using the Habit Tracker is presented with a warning that "The responsibility for keeping the records correct lies on you." and prompted to press any key before being presented with the main menu.



## My thought process

## What was hard?

## What was easy?

## What have I learned?
