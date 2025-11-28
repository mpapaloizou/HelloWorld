Notes : 
1) For Requests / Responses I am using "records" and "init" for immutability (so that DTO's do not change after creation)
2) Using 'Result Pattern' (Design pattern) in OpenWeatherService - ErrorOr is one library that implements it
3) Trying to use the 'Return Early' (Design pattern) and return errors early if any, and the (successful) result in the last line, so it is consistent and easy to read / know what to expect
4) For architecture I've used Vertical Slice/Slicing Architecture (basically folders Common and Features), but in other projects (side projects or work) I often use Clean Architecture combined with Vertical Slice Architecture (basically more projects and keeping the 'Common' and 'Features' folders).
     - I know what is Domain Driven Design Architecture and MicroServices but never used them.


Enchasments / Improvements that I would do if I invested more time :

Other : 
1) Convert this ReadMe.txt file to ReadMe.md 

API :
1) Logger & Open telemetry to log errors for observability
2) ExceptionMiddlewere to catch errors, return 500 and a user friendly message (instead of exception messages which are sensitive data and not user friendly) and log
3) GlobalExceptionHandler
4) FluentValidators for Requests (e.g. in API endpoint - WeatherRequest to verify it's not null)
5) MediatR pattern & CQRS 
      - With behaviors like logging, exception, performance, validation (using the FluentValidators)
      - CQRS  -  Command and queries segmentation

6) Customize ErrorOr (e.g. create custom result types) and return the appropriate status codes based on type of error
e.g.  
//if (errorOrResult.FirstError.Type.Equals(Error.Unexpected()))
//{
// return 500
//} else if ValidationError
// return BadRequest() 
// ...

7) Use AppSettings & .env files
8) Dockerize and make sure that communication works across different setups (e.g. dockerize API and run react from development environment (VS code). Override appropriate variables)
9) I personally like 'SmartEnum' library instead of normal Enums
10) MockExternalWeatherService needs improvements. This is very important for PARAMETERISED testing.
	  It must mimic the functionality of the API and return random (but correct) data so we can catch errors
Bug -->  e.g. What happens in the scenario when it is over 25°C but also raining ? Shall I return both "It’s a great day for a swim" and "Don’t forget the umbrella" ? 
         If tested with the real API without random / parameterized tests / random data - I would probably miss the bug (if I didn't think about it)
         I realized this bug from the beggining, when I wrote RecommendationPhraseConstantStrings
11) APIKey in OpenWeatherService - Must read it from .env files or environment variables (git ignored), or visual studio secrets
     - We can create a custom class and register it as a service, and validate on startup that this is given from .env file / envrioment variable / Visual studio secrets / appsettings
12) Make a custom CLI template - basically instead of creating a new Web API with the default code, create a new project based on a custom template, e.g. with ExceptionMiddlewere, GlobalExceptionHandler, Logger/OpenTelemetry, etc  (Common folder and maybe 1 feature entity for example)
13) Server side caching (and cache invaldiation) especially for this project
14) Could use SignalR for real time (I've used it with my last 2 employers) so that if there's a change in the weather to update the UI in real time without refresh, or
15) Functionality to send notifications - can be done with SignalR. Or Push Notifications if it was a mobile app (e.g. via FireBase)

Tests (API + Unit): 
1) Use XUnit
2) Use Parameterized tests to catch bugs like the one mentioned before, number "10", if the goal is to catch bugs
3) Use interface / mock service (with a good implementation) to not spam the API, mimic the API & have a variety of different results
4) Use interface / mock service to skip some functionality (e.g. there might be a flow of 5 steps and payment is 4. Maybe we don't wanna test the payment part, but everything else)
5) Use interface / mock service for unit tests / isolation
6) Use Faker / Bogus for random data to catch bugs like number "10"
7) Decide whether the tests should use CustomWebApplicationFactory or not - up to the Seniors / Team leader
      - I think that if we do not use CustomWebApplicationFactory then the environment is closer to production, but I might be wrong

UI Tests (Acceptance tests / UI tests / End to End tests) :
8) Some UI tests on Web / React, for verification and test coverage, but those tests are expensive to write and slow so not ideal to catch bugs like number "10"


