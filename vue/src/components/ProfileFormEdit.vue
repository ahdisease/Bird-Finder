<template>
  <div v-show="displayForm === true" id="profile-form-edit">
    <form v-on:submit.prevent="editProfile">
      <!-- to do: make this so it can swap the image dynamically when they upload -->
      <!-- <img v-bind:src="profile.profileImg" id="profile-img" /> -->
      <!-- <label for="img-url"
        >Customize your profile photo by adding an image URL.</label
      >
      <input
        id="img-url"
        placeholder=" Add an image URL here."
        type="text"
        size="50"
        v-model="profile.profileImg"
      /> -->
      <!-- <button id="upload-profile-image" v-on:click="updateImage">Upload</button> -->
      <label for="fav-bird">What is your favorite bird?</label>
      <select
        required
        id="fav-bird"
        v-model="profile.favoriteBird"
      >
        <option v-for="bird in birds" v-bind:key="bird.id" v-bind:value="bird">{{ bird.name }}</option>
      </select>

      <label for="most-common-bird"
        >Tell us what bird you most commonly spot.</label
      >
      <select
        required
        id="most-common-bird"
        v-model="profile.mostCommonBird"
      >
      <option v-for="bird in birds" v-bind:key="bird.id" v-bind:value="bird">{{ bird.name }}</option>
      </select>

      <label for="zip-code">Please enter your zip code:</label>
      <input
        required
        id="zip-code"
        type="text"
        pattern="[0-9]{5}"
        title="Numbers only, please!"
        placeholder="#####"
        v-model="profile.zipCode"
      />

      <label for="skill-lvl">What is your skill level?</label>
      <select required id="skill-lvl" size="3" v-model="profile.skillLevel">
        <option value="beginner">Beginner</option>
        <option value="intermediate">Intermediate</option>
        <option value="advanced">Advanced</option>
      </select>

      <input type="submit" />
    </form>
  </div>
</template>


<script>
import profileService from "../services/ProfileService";
import birdService from "../services/BirdService";
export default {
  data() {
    return {
      profile: {
        username: this.$store.state.user.username,
      },
      birds: [],
      displayForm: true,
      error:''
    };
  },
  methods: {
    updateImage() {},
    editProfile() {
      this.displayForm = false;
      profileService
        .editProfile(this.profile)
        .then((response) => {
          // By default, if no errors are encountered, handler methods respond with a status code of 200
          if (response.status === 200) {
            this.$store.commit("SET_PROFILE", this.profile);
            this.$router.go(0);
          }
        })
        .catch((err) => {
          this.error=err + " problem updating profile!";
        });
    },
    getBirds() {
      birdService
        .getBirds()
        .then((response) => {
          if (response.status === 200) {
              this.birds = response.data
          }
        })
    }
  },
  mounted() {
    this.getBirds();
  }
};
</script>

<style scoped>
#profile-form-edit {
  border: 4px solid #ff9f1c;
  padding: 25px 25px 35px 25px;
  background-color: #011627;
  color: #fdfffc;
  width: 75%;
}
form {
  display: flex;
  flex-direction: column;
  gap: 12px;
  align-items: center;
}
label {
  font-family: "Bitter", serif;
  font-size: 1.3rem;
}
input[type="zip-code"] {
  width: 45%;
}
#skill-lvl {
  width: 45%;
}
input,
select,
#skill-lvl {
  padding: 8px;
  border-radius: 8px;
  font-size: 1rem;
  border: 1px solid #011627;
  border-left: 5px solid #ff9f1c;
  border-right: 5px solid #ff9f1c;
}
input[type="submit"] {
  margin-top: 25px;
  width: 45%;
  border: 1px solid #011627;
  background-color: #e71d36;
  font-size: 1rem;
  font-weight: bold;
  border-radius: 8px;
  border-left: 5px solid #ff9f1c;
  border-right: 5px solid #ff9f1c;
}
select {
  width:45%;
}
</style>