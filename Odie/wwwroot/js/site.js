var odie = {
    helpers: {
        /* Helper to generate unique id. */
        generateGuid: function () {
            function s4() {
                return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
            }
            return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
        }
    },
    ui: {
        /* Show temporary popover below item. */
        pop: function(el, text) {
            el.popover({
                placement: 'bottom',
                content: text,
                trigger: 'manual'
            });
            el.popover('show');
            
            setTimeout(function () {
                el.popover('hide');
                el.popover('destroy');
            }, 1000);
        }        
    }
}