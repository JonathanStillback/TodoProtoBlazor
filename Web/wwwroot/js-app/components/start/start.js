console.log("start component loaded");

class Start extends JsAppComponent
{
	constructor()
	{
		super();
		this.hello = "dynamically rendered hello from start component";
		this.list = [{word: "some"}, {word: "random"}, {word: "words"}];
	}
}
new Start();