Programmers should generate their websites. This is my attempt.

Websites generated with SiteSharper are:

	http://www.brainsharper.com
	[my next generation blog]

SiteSharper uses a combination of Markdown and Razor templates to create static HTML pages. 

The Markdown and XML Razor templates can refer to arbitrary "modules" and may pass parameters to them. 

Modules can be - again - Markdown or Razor templates. The engine expands each module recursivly until no more modules can be found and then converts the result to HTML.
