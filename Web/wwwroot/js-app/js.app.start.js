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
		"": "js-app/components/start",
		"/": "js-app/components/start",
		"/todos": "js-app/components/todo"
	};
	return routes;
})();

var jsComponents = {};

console.log(`jsApp - Location: ${document.location.pathname}`);

function getFilePath(path)
{
	let slash = path.lastIndexOf("/");
	let componentName = path.substr(slash+1);
	return {view: `${path}/${componentName}.html`, script: `${path}/${componentName}.js`, componentName: componentName};
}

class JsAppComponent {
	constructor()
	{
		console.log("JsAppComponent loaded");
		this._content = "content not fetched";
		jsComponents[this.constructor.name.toLowerCase()] = this;
		this.getContentFile(this);
		this.enableClientRouting();
	}

	async getContentFile(o)
	{
		fetch(getFilePath(jsRoutes[document.location.pathname]).view, {})
		.then(function (response) {
			if (response.ok) {
				return response.text();
			} else {
				return Promise.reject(response);
			}
		})
		.then(function(data) {
			o._content = data;
			o.render();
		})
		.catch(function (err) {
			console.warn('Something went wrong.', err);
		});
	}
	
	parseContent()
	{
		this.parseList();
		this._content = this.parseInline(this._content, this);
	}

	parseList()
	{
		let matches = this.getMatchAll(this._content, /<(.+) .*jsFor="(.+)".*>(.*)<\/.+>/);
		for (let i = 0; i < matches.length; ++i)
		{
			let m = matches[i];

			let el = m[1];
			let model = m[2];
			let value = m[3];

			let r = "";
			for (let k = 0; k < eval("this."+model).length; ++k)
			{
				let item = eval("this."+model)[k];

				let parsedValue = this.parseInline(value, item);

				r += `<${el}>${parsedValue}</${el}>`;
			}
			this._content = this.replaceAll(this._content, m[0], r);
		}
	}

	parseInline(str, container)
	{
		let matches = this.getMatchAll(str, /{{(.+)}}/);
		for (let i = 0; i < matches.length; ++i)
		{
			let m = matches[i];
			str = this.replaceAll(str, m[0], eval("container"+"."+m[1]))
		}
		return str;
	}

	getMatchAll(str, regex)
	{
		let matches = [];
		let m = {};
		while (m != null)
		{
			m = str.match(regex);
			if (m != null)
			{
				let i = 0;
				while(i > -1)
				{
					i = str.indexOf(m[0]);
					if (i > -1)
						str = str.replace(m[0],"");
				}

				matches.push(m);
			}
		}
		return matches;
	}

	replaceAll(str, search, repl)
	{
		let i = 0;
		while (i > -1)
		{
			i = str.indexOf(search);
			if (i > -1)
				str = str.replace(search, repl);
		}
		return str;
	}

	render()
	{
		this.parseContent();
		jsApp.innerHTML = this._content;
	}

	static loadComponent()
	{
		let comp = jsComponents[getFilePath(jsRoutes[document.location.pathname]).componentName];
		if (comp)
		{
			comp.getContentFile(comp);
			return;
		}
		var script = document.createElement('script');
		script.onload = function () {
				//do stuff with the script
		};
		script.src = getFilePath(jsRoutes[document.location.pathname]).script;
		
		document.head.appendChild(script); //or something of the likes
	}

	enableClientRouting()
	{
		let links = $("a");
		links.forEach(
			l =>
			{
				l.addEventListener("click",
				function(e)
				{
					e.preventDefault();
					history.pushState("", "", this.href);
					JsAppComponent.loadComponent();
					// console.log("clicked link");
				});
			}
		);
	}
}

// JsAppComponent.loadComponent();
JsAppComponent.loadComponent();