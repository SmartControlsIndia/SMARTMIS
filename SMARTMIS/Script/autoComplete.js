// ########## STORES THE XMLHTTP OBJECT
var xmlhttp = createxmlhttp();

// ########## CREATES AN XMLHTTP OBJECT
function createxmlhttp() {
    var xmlhttp;

    if (window.ActiveXObject) { // INTERNET EXPLORER
        try {
            xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
        } catch (e) {
            xmlhttp = false;
        }
    } else { // OTHER BROWSERS
        try {
            xmlhttp = new XMLHttpRequest();
        } catch (f) {
            xmlhttp = false;
        }
    }

    if (!xmlhttp) { // RETURN THE OBJECT OR DISPLAY ERROR
        alert('there was an error creating the xmlhttp object');
    } else {
        return xmlhttp;
    }
}

// ########## MAKE AN ASYNCHRONOUS CALL USING THE XMLHTTP OBJECT
// ########## THE VARIABLE STR BEING THE TEXT ALREADY ENTERED
function searchforwords(str) {

    try {
        // PROCEED ONLY IF OBJECT IS NOT BUSY
        if (xmlhttp.readyState === 4 || xmlhttp.readyState === 0) {
            // EXECUTE THE PAGE ON THE SERVER AND PASS QUERYSTRING
            xmlhttp.open("POST", "../autocomplete.aspx", true);
            
            // DEFINE METHOD TO HANDLE THE RESPONSE
            xmlhttp.onreadystatechange = handleresponse;
            // MAKE CALL
            xmlhttp.send(null);

        } else {
            // IF CONNECTION IS BUSY, WAIT AND RETRY
        setTimeout('searchforwords("' + str + '")', 1000);
        }
    } catch (e) {
        //alert('ERROR: ' + e);
    }
}

// ########## EXECUTED WHEN MESSAGE IS RECIEVED
function handleresponse() {
    try {
        // MOVE FORWARD IF TRANSACTION COMPLETE
        if (xmlhttp.readyState == 4) {
            // STATUS OF 200 INDICATES COMPLETED CORRECTLY
            if (xmlhttp.status == 200) {
                // WILL HOLD THE XML DOCUMENT
                var xmldoc;
                if (window.ActiveXObject) { // INTERNET EXPLORER
                    xmldoc = new ActiveXObject("Microsoft.XMLDOM");
                    xmldoc.async = "false";
                    xmldoc.loadXML(xmlhttp.responseText);
                } else { // OTHER BROWSERS
                    var parser = new DOMParser();
                    xmldoc = parser.parseFromString(xmlhttp.responseText, "text/xml");
                }

                // PARSE EACH RESULT
                var res = xmldoc.getElementsByTagName('viHMIGTBarcodeTextBox');

                if (res.length == 0) {
                 }
                else {

                    var GTBarcode = (res[0].childNodes[0].nodeValue).substring(0, 10)
                    var Result = (res[0].childNodes[0].nodeValue).substring(10, 11)

                    document.getElementById('viHMIGTBarcodeTextBox').value = GTBarcode;

                    if (Result == "Y") {
                        document.getElementById('viHMIStatusButton').value = "OK";
                    }
                    else if (Result == "N") {
                        document.getElementById('viHMIStatusButton').value = "Not OK";
                    }
                }

//                for (var i = 0; i < res.length; i++) {
//                    
//                    document.getElementById('viHMIGTBarcodeTextBox').value = (res[i].childNodes[0].nodeValue); // DO SOMETHING WITH THIS VALUE
//                }

            } else { // STATUS OTHER THAN 200 IS AN ERROR
            //alert('there was an error recieving the message');
            //alert(xmlhttp.status);
            }
        }
    } catch (e) {
        //alert('there was an error contacting the server');
        //alert(e)
    }
}