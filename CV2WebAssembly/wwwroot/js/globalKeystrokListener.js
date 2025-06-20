window.globalKeyListener = {
    initialize: function (dotnetHelper) {
        // listen to all keydown events
        window.addEventListener('keyup', function (evt) {
            // ignore modifier-only presses
            if (evt.ctrlKey || evt.altKey || evt.metaKey) return;

            // We're interested in the number row—not numpad—so use evt.key
            const k = evt.key;
            if (k === '1' || k === '2' || k === '3' || k === '4') {
                // prevent default if you don’t want the page to scroll, etc.
                evt.preventDefault();

                // call into .NET: pass the key as a string
                //dotnetHelper.invokeMethodAsync('MyApp', 'OnGlobalNumberKey', k);
                alert('Pressed');
            }
        });
    }
};
