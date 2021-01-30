function downloadFile(filename)
{
	window.location.href = filename;
	document.getElementById("backdiv").remove();
	document.getElementById("innerdiv").remove();	
}
function closebox()
{
	document.getElementById("backdiv").remove();
	document.getElementById("innerdiv").remove();
}