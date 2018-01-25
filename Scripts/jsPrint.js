function CallPrint(strid) {
    var dir = ('<%= Session["culture"] %>' == "en-US") ? 'ltr' : 'rtl';
    var prtContent = document.getElementById(strid);
    var WinPrint = window.open('', '', 'letf=0,top=0,width=400,height=400,toolbar=0,scrollbars=0,status=0');
    WinPrint.document.write("<html dir=" + dir + "><head><link href='../CSS/TableStyle.css' rel='stylesheet' type='text/css' /></head><body><table dir=" + dir + ">" + prtContent.innerHTML + "</table></body></html>");
    WinPrint.dir = dir;
    WinPrint.document.close();
    WinPrint.focus();

    WinPrint.print();
    // WinPrint.close();
}