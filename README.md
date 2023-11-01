# Bird Nerds

Bird Nerds is a hobby web app that allows users to post about birds they have seen at their local bird feeder. Bird sightings that have been posted store the birds so that they may be found in a search by zipcode. Users can look back at their reported sightings by viewing their saved lists.

This project is built using C#/.NET and SQL Server for the back end API, and the Vue.js front end was provided as part of a Tech Elevator alumni project. In general, the front end has been left untouched in style, though minor structural edits may be made to adapt to the format of the back end API. Project partner:
- [kimbambala](https://github.com/kimbambala)

## Schema
The diagram below describes the database schema.

<img
    alt="Entity relationship diagram."
    src="./screenshots/Bird_Nerd_ERD.png"
    width=700
/>

## API Routes 
The following tables describe all available API endpoints. Endpoint URLs were chosen to match Vue front end supplied by Tech Elevator.
### User/Profile
| HTTP Method | Endpoint URL | Description | Status code | Returned Value |
| :---: | :---: | :--- | :---: | :--- | 
|**POST**|'/register'| Register a new user. | 201 | {<br/>&emsp;"userId",<br/>&emsp;"username",<br/>&emsp;"role"<br />} |
|||| 409, 500 | { "message" } |
|**POST**|'/login'| Request a JWT for authorization. | 200 | {<br/>&emsp;user: {<br/>&emsp;&emsp;"userId",<br/>&emsp;&emsp;"username",<br/>&emsp;&emsp;"role"<br/>&emsp;},<br/>&emsp;"token"<br/>} |
|||| 409, 500 |  { "message" }  |
|**GET**|'/profile'| Request the current user's profile information. | 200 |  {<br/>&emsp; "zipcode",<br/>&emsp; "skillLevel",<br/>&emsp; "favoriteBird",<br/>&emsp; "mostCommonBird",<br/>&emsp; "profileActive"<br/> }  |
|||| 404 |  { "message" }  |
|**POST**|'/createProfile'| Create a profile for the current user based on a JSON object in the body. Also reactivates a deleted profile. | 201 | { <br/>&emsp;"zipcode", <br/>&emsp;"skillLevel", <br/>&emsp;"favoriteBird", <br/>&emsp;"mostCommonBird", <br/>&emsp;"profileActive" <br/>} |
|||| 400 | { "message" }  |
|**PUT**|'/editProfile'| Updates a profile for the current user based on a JSON object in the body.| 200 |  |
|||| 400 | { "message" }  |
|**DELETE**|'/deleteProfile'| Deactivates a profile for the current user. | 204 |  |
|||| 404 | { "message" }  |
---
---
<br/>

### List
| HTTP Method | Endpoint URL | Description | Status code | Returned Value |
| :---: | :---: | :--- | :---: | :--- | 
|**GET**|'/lists/`{listId}`'| Request a user owned list of birds with given ID. | 200 | {<br/>&emsp;"listId",<br/>&emsp;"userId",<br/>&emsp;"listName"  <br/>} |
|||| 404 | { "message" } |
|**GET**|'/lists'| Request all user owned lists of birds. |  | [<br/>&emsp;{<br/>&emsp;&emsp;"listId",<br/>&emsp;&emsp;"userId",<br/>&emsp;&emsp;"listName"  <br/>&emsp;},<br/>&emsp;. . .<br/>] |
|**POST**|'/createList'| Create a new list from JSON object. | 201 ||
|||| 400 | { "message" } |
|**PUT**|'/editList'| Update a list's name with JSON object. | 200 | |
|||| 400, 404 | { "message" } |
|**DELETE**|'/deleteList/`{listId}`'| Delete a user owned list with a given ID. Also deletes all birds and notes tied to the list. | 204 ||
|||| 400 | { "message" } |
---
---
<br/>

### Bird 
| HTTP Method | Endpoint URL | Description | Status code | Returned Value |
| :---: | :---: | :--- | :---: | :--- | 
|**GET**|'/birds'| Request an array of all birds available in the database. | 200 | [<br/>&emsp; {<br/>&emsp;&emsp;"birdId",<br/>&emsp;&emsp;"listId", <br/>&emsp;&emsp;"birdName", <br/>&emsp;&emsp;"imgUrl", <br/>&emsp;&emsp;"zipCode"<br/>&emsp;},<br/>&emsp;  . . . <br/>] |
|||| 400, 404 | { "message" } |
|**GET**|'/lists/`{listId}`/birds'|Request an array of all birds from a list with a specific list ID.| 200 | [<br/>&emsp; {<br/>&emsp;&emsp;"birdId",<br/>&emsp;&emsp;"listId", <br/>&emsp;&emsp;"birdName", <br/>&emsp;&emsp;"imgUrl", <br/>&emsp;&emsp;"zipCode"<br/>&emsp;},<br/>&emsp;  . . . <br/>]|
|||| 400, 404 | { "message" }|
|**GET**|'/birds/`{zipCode}`'|Request an array of all birds seen at a given zipcode.| 200 | [<br/>&emsp; {<br/>&emsp;&emsp;"birdId",<br/>&emsp;&emsp;"listId", <br/>&emsp;&emsp;"birdName", <br/>&emsp;&emsp;"imgUrl", <br/>&emsp;&emsp;"zipCode"<br/>&emsp;},<br/>&emsp;  . . . <br/>]|
|||| 400, 404 | { "message" }|
|**GET**|'/birds/`{id}`'| Request a bird JSON object with a specific ID. | 200 | {<br/>&emsp;"birdId",<br/>&emsp;"listId", <br/>&emsp;"birdName", <br/>&emsp;"imgUrl", <br/>&emsp;"zipCode"<br/>} |
|||| 400, 404 | { "message" }|
|**GET**|'/randomBird'| Request a random bird. | 200 | {<br/>&emsp;"birdId",<br/>&emsp;"listId", <br/>&emsp;"birdName", <br/>&emsp;"imgUrl", <br/>&emsp;"zipCode"<br/>} |
|||| 400, 404 | { "message" }|
|**POST**|'/birds'| Create a bird from a JSON object. | 201 | |
|||| 400 | { "message" }|
|**PUT**|'/birds/{id}'| Update a user owned bird to match a JSON object. | 200 |  |
|||| 400 | { "message" }|
|**DELETE**|'/bird/{id}'| Delete a user owned bird from the database with a specific ID. Also deletes all notes tied to the bird. | 204 |  |
|||| 400 | { "message" }|
---
---
<br/>

### Note
| HTTP Method | Endpoint URL | Description | Status code | Returned Value |
| :---: | :---: | :--- | :---: | :--- | 
|**GET**|'/bird/`{birdId}`/notes'| Request an array of all notes for a bird. | 200 | [<br/>&emsp; {<br/>&emsp;&emsp;"noteId",<br/>&emsp;&emsp;"birdId",<br/>&emsp;&emsp;"dateSpotted",<br/>&emsp;&emsp;"numMales",<br/>&emsp;&emsp;"numFemales",    <br/>&emsp;&emsp;"feederType",    <br/>&emsp;&emsp;"foodBlend",    <br/>&emsp;&emsp;"notes"<br/>&emsp;},<br/>&emsp;  . . . <br/>] |
|||| 400, 404 | { "message" } |
|**GET**|'/note'| Request a note with a specific ID as a JSON object. | 200 | {<br/>&emsp;"noteId",<br/>&emsp;"birdId",<br/>&emsp;"dateSpotted",<br/>&emsp;"numMales",<br/>&emsp;"numFemales",    <br/>&emsp;"feederType",    <br/>&emsp;"foodBlend",    <br/>&emsp;"notes"<br/>} |
|||| 400, 404 | { "message" } |
|**POST**|'/newNote'| Create a note that matches a JSON object.  | 201 |  |
|||| 400 | { "message" } |
|**PUT**|'/editNote'| Update a user owned note to match a JSON object.  | 200 |  |
|||| 400 | { "message" } |
|**DELETE**|'/deleteNote/`{id}`'| Delete a user owned note with a specific ID.  | 204 |  |
|||| 400 | { "message" } |
---
---
<br/>




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

When profiles are created, a skill level of 'beginner' is automatically assigned. A user can update their favorite bird, their most commonly spotted bird, their zip code, and their skill level at any time.

<img
    alt="Screenshot of profile update form. Four questions: 1) What is your favorite bird? 2) Tell us what you most commonly spot. 3) Please enter your zip code: 4) What is your skill level? (Beginner, Intermediate, Advanced)"
    src="./screenshots/update_profile_form.png"
    width=400
/>

## Building a List and Reporting a Bird
WIP - this section is currently being constructed.
