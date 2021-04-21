mergeInto(LibraryManager.library, {
  GetParameterFromBrwoser: function (parameterName) {
    var url_string = document.location.href;
    var url = new URL(url_string);
    var param = url.searchParams.get(Pointer_stringify(parameterName));
    console.log("Fetched Data from within Unity JavaScript (" + Pointer_stringify(parameterName) + "): " + param);

    if (param != null && param != "") {
      var bufferSize = lengthBytesUTF8(param) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(param, buffer, bufferSize);
      return buffer;
    } else {
      return null;
    }
  },
});
