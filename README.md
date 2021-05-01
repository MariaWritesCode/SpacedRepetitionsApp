## SpacedRepetitionsApp

### Overview
Spaced repetition is a study technique of repeating study material in intervals. You can read more about the technique [here](https://en.wikipedia.org/wiki/Spaced_repetition).

This is my attempt (during [Tech Leaders Program](https://techleaders.eu/)) of writing a .NET 5 / Blazor application for spaced repetitions using:

* .NET 5 backend with Entity Framework Core, Redis on Azure, Azure Functions (tbd, for revision-notification mechanism), SQL Server
* Blazor for front end
* App is deployed on Azure App Service with SQL server and Azure-hosted database.

The application will allow grouping flashcards ("Notes") into Categories and tagging notes with tags. Every 1, 5, 7, 16 and 35 days after note creation, the flashcard to repeat will appear on the main screen. Once it is revised, time to next repetition starts running.

### Current progress
1. Authorization/Identity has just been added and UserId foreign key was added to entities. Controllers and services in API will be modified to include UserId while creating new items.
2. Repetition mechanism is yet to be implemented - there will be a service that will pick up notes by repetition dates and display them on the main screen (probably an Azure Function). 
3. Unit tests for repetition mechanism and tag duplication prevention need to be written - for now there is not much to unit test.
4. Blazor UI is very much in progress:
   * There are three services for data manipulations (one for Note, Tag and Categories each).
   * Custom components have been added for: Categories List in navigation menu, Category Form for adding new categories, NoteForn for adding/editing notes, TagSection for tag adding/edit on NotForm and CardComponent for note display as flashcards
   * What needs to be done further: adding identity related pages to navigation, navigation list automatic refresh, flashcard pop-up, tag edit needs to be fixed
   * I plan on adding some nicer themes and maybe get rid of Blazorise and use "clear" bootstrap components

### Deployment / Services used
1. The app uses an Azure SQL Server & SQL Database
2. I started implementing redis cache usage and use redis cache for Azure
3. Azure App Configuration is used for configuration (locally I use [AppSecrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-5.0&tabs=windows))
4. API and Blazor projects are deployed on 2 different App Services
5. Azure DevOps for Pipelines and Release

