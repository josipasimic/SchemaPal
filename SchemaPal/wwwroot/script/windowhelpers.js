function exportDivToPng(divId) {
    var element = document.getElementById(divId);
    html2canvas(element).then(canvas => {
        var link = document.createElement('a');
        link.download = 'export.png';
        link.href = canvas.toDataURL();
        link.click();
    });
}

window.saveAsFile = function (filename, content) {
    var blob = new Blob([content], { type: 'application/json' });

    var link = document.createElement('a');
    link.href = URL.createObjectURL(blob);
    link.download = filename;

    document.body.appendChild(link);
    link.click();

    document.body.removeChild(link);
}