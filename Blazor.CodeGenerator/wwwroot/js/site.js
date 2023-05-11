function urlFor(path) {
    var prefijo = window.location.href;
    var sufijo = path;

    if (!prefijo.endsWith("/")) {
        prefijo += "/";
    }
    if (sufijo.startsWith("/")) {
        sufijo = sufijo.substr(1);
    }
    return prefijo + sufijo;
}