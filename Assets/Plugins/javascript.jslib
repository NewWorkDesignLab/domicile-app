mergeInto(LibraryManager.library, {

  GetScenarioInformationFromBrwoser: function () {
    var url_string = document.location.href;
    var url = new URL(url_string);
    var scenario = url.searchParams.get("scenario");
    console.log("Fetched Data from within Unity: " + scenario);

    var bufferSize = lengthBytesUTF8(scenario) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(scenario, buffer, bufferSize);
    return buffer;
  },

  GetPlayerInformationFromBrwoser: function () {
    var url_string = document.location.href;
    var url = new URL(url_string);
    var is_player = url.searchParams.get("is_player");
    console.log("Fetched Data from within Unity: " + is_player);

    var bufferSize = lengthBytesUTF8(is_player) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(is_player, buffer, bufferSize);
    return buffer;
  },
});
