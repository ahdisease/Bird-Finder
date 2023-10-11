# Bird Nerds - WIP

Bird Nerds is a hobby web app that allows users to post about birds they have seen at their local bird feeder. Bird sightings that have been posted store the birds so that they may be found in a search by zipcode. Users can look back at their reported sightings by viewing their saved lists.

This project is built using C#/.NET and SQL Server for the back end API, and the Vue.js front end was provided as part of a Tech Elevator alumni project. In general, the front end has been left untouched in style, though minor structural edits may be made to adapt to the format of the back end API. Project partner:
- [kimbambala](https://github.com/kimbambala)

## Schema - WIP
The diagram below describes the database schema. Currently there is no List item.

<img
    alt="Entity relationship diagram."
    src="./screenshots/Bird_Nerd_ERD.png"
    width=700
/>

## API Routes
The following actions are available using the API:

| HTTP Method | Endpoint URL[^1] | Status code | Returned Value |
| :---: | :---: | :---: | :--- | 
|**POST**|'/register'| 201 | ``` { "userId", "username", "role" } ``` |
||| 409, 500 | ``` { "message" } ``` |
|**POST**|'/login'| 200 | ``` { user: { "userId", "username", "role" }, "token" }``` |
||| 409, 500 | ``` { "message" } ``` |
|**GET**|'/profile'| 200 | ``` { "zipcode", "skillLevel", "favoriteBird", "mostCommonBird", "profileActive" } ``` |
| || 404 | ``` { "message" } ``` |
|**POST**|'/createProfile'| 201 | ``` { "zipcode", "skillLevel", "favoriteBird", "mostCommonBird", "profileActive" } ``` |
|**PUT**|'/updateProfile'| 200 |  |
|**DELETE**|'/deleteProfile'| 204 |  |
|**DELETE**|'/deleteProfile'| 404 | ``` { "message" } ``` |
|**GET**|'/birds'| N/A[^2] | ```[ { "id", "name", "description", "imgUrl" }, ... ]``` |
| || | ``` [] ``` |
|**GET**|'/birds/{id}'| N/A[^2] | ``` { "id", "name", "description", "imgUrl" } ``` |
|**GET**|'/randomBird'| N/A[^2] | ``` { "id", "name", "description", "imgUrl" } ``` |
|**POST**|'/birds'| N/A[^2] | |
|**PUT**|'/birds/{id}'| N/A[^2] |  |
|**DELETE**|'/bird/{id}'| N/A[^2] |  |

[^1]: Endpoint URLs were chosen to match Vue frontend supplied by Tech Elevator.
[^2]: Bird related actions currently return no status codes

## Profile creation
An anonymous user arrives at the landing page as shown below:

<img 
    alt="Screenshot of the landing page. The landing page is split into four quadrants: top left is the Bird Nerd logo. Top right is the app title 'BIRD NERDS'. The bottom left is a navigation bar consisting of a search button, a login button, a register button. The bottom right, taking up the majority of the screen, is an image of a bird." 
    src="./screenshots/landing_page.png" 
    width=700 
/>

The landing page displays a random image of a bird that has been stored in the database.

The user creates their account with the form that appears after selecting the REGISTER button in the navigation bar on the left. This automatically creates an empty profile that the user can customize.

<img 
    alt="Screenshot of the registration form that generates after selecting the REGISTER button" 
    src="./screenshots/registration_box.png" 
    width=275 
/>

After logging in, the user can navigate to their profile using the new button in the navigation bar.

<img 
    alt="Screenshot of the profile page. The web page is identical in structure to the landing page (though the buttons in the navigation bar for a logged in user consist of 'SEARCH', 'MY LISTS', 'PROFILE', and 'HOME'). In the bottom right, taking up the majority of the screen, is the profile page, consisting of a welcome message to the logged in user, a button labeled 'Edit your profile', and a table that lists four items: 'My favorite bird', 'Most common bird at my feeder', 'My skill level', and 'My zip code'. All items other than 'My skill level' have blank entries because a new user has just viewed their profile for the first time." 
    src="./screenshots/profile_page.png" 
    width=700
/>

When profiles are created, a skill level of 'beginner' is automatically assigned. A user can update their favorite bird, their zip code, and their skill level at any time[^3].

<img
    alt="Screenshot of profile update form. Four questions: 1) What is your favorite bird? 2) Tell us what you most commonly spot. 3) Please enter your zip code: 4) What is your skill level? (Beginner, Intermediate, Advanced)"
    src="./screenshots/update_profile_form.png"
    width=400
/>

[^3]:WIP note: The back end API calculates the most common bird at the user's feeder on profile update. The front end currently includes a selection box, but it has no effect on the user's profile.

## Building a List and Reporting a Bird
WIP - this section is currently being constructed.