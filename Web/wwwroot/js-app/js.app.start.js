console.log("Hello World jsApp");

var $ = document.querySelectorAll;
var $ = document.querySelectorAll.bind(document);

console.log("configured $ query");

var jsApp = (function()
{
	let q = $("app");
	let jsApp = (q.length > 0) ? q[0] : null;
	return jsApp;
})();

var jsRoutes = (function()
{
	let routes =
	{
		"": "js-app/components/start/index.html",
		"/": "js-app/components/start/index.html",
		"/todos": "js-app/components/todo"
	};
	return routes;
})();

console.log(`jsApp - Location: ${document.location.pathname}`);

fetch(jsRoutes[document.location.pathname], {})
.then(function (response) {
	// The API call was successful!
	if (response.ok) {
		return response.text();
	} else {
		return Promise.reject(response);
	}
})
.then(function (data) {
	// This is the JSON from our response
	jsApp.innerHTML = data;
})
.catch(function (err) {
	// There was an error
	console.warn('Something went wrong.', err);
});