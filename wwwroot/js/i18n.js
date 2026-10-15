/**
 * Human Square — i18n engine
 * Charge les traductions depuis /lang/{locale}.json
 * et remplace le contenu des éléments [data-i18n].
 *
 * Attributs supportés :
 *   data-i18n="key"               → remplace textContent
 *   data-i18n-placeholder="key"   → remplace placeholder
 *   data-i18n-title="key"         → remplace title
 *   data-i18n-html="key"          → remplace innerHTML (pour balises internes)
 */
(function () {
    'use strict';

    var SUPPORTED = ['fr', 'pt', 'en'];
    var DEFAULT   = 'fr';
    var CACHE     = {};

    /* ── Détection de la langue ── */
    function getLocale() {
        // 1. cookie "hs_lang"
        var m = document.cookie.match(/(?:^|;\s*)hs_lang=([a-z]{2})/);
        if (m && SUPPORTED.indexOf(m[1]) !== -1) return m[1];
        // 2. query string ?lang=xx
        var url = new URL(window.location.href);
        var qp  = url.searchParams.get('lang');
        if (qp && SUPPORTED.indexOf(qp) !== -1) return qp;
        // 3. navigator
        var nav = (navigator.language || '').substring(0, 2).toLowerCase();
        if (SUPPORTED.indexOf(nav) !== -1) return nav;
        return DEFAULT;
    }

    function setLocale(lang) {
        if (SUPPORTED.indexOf(lang) === -1) return;
        document.cookie = 'hs_lang=' + lang + ';path=/;max-age=31536000;SameSite=Lax';
        applyTranslations(lang);
    }

    /* ── Chargement du JSON ── */
    function loadTranslations(lang, cb) {
        if (CACHE[lang]) return cb(CACHE[lang]);
        var xhr = new XMLHttpRequest();
        xhr.open('GET', '/lang/' + lang + '.json?v=' + Date.now(), true);
        xhr.onload = function () {
            if (xhr.status === 200) {
                try { CACHE[lang] = JSON.parse(xhr.responseText); } catch (e) { CACHE[lang] = {}; }
            } else { CACHE[lang] = {}; }
            cb(CACHE[lang]);
        };
        xhr.onerror = function () { CACHE[lang] = {}; cb(CACHE[lang]); };
        xhr.send();
    }

    /* ── Application des traductions ── */
    function applyTranslations(lang) {
        loadTranslations(lang, function (dict) {
            // data-i18n → textContent / innerHTML
            var els = document.querySelectorAll('[data-i18n]');
            for (var i = 0; i < els.length; i++) {
                var key = els[i].getAttribute('data-i18n');
                if (dict[key] !== undefined) {
                    if (els[i].hasAttribute('data-i18n-html')) {
                        els[i].innerHTML = dict[key];
                    } else {
                        els[i].textContent = dict[key];
                    }
                }
            }
            // data-i18n-placeholder
            var ph = document.querySelectorAll('[data-i18n-placeholder]');
            for (var j = 0; j < ph.length; j++) {
                var pk = ph[j].getAttribute('data-i18n-placeholder');
                if (dict[pk] !== undefined) ph[j].placeholder = dict[pk];
            }
            // data-i18n-title (attribut title)
            var tl = document.querySelectorAll('[data-i18n-title]');
            for (var k = 0; k < tl.length; k++) {
                var tk = tl[k].getAttribute('data-i18n-title');
                if (dict[tk] !== undefined) tl[k].title = dict[tk];
            }
            // Mettre à jour <html lang>
            document.documentElement.lang = lang;
            // Mettre à jour le bouton de langue
            var langBtn = document.getElementById('hsLangBtn');
            if (langBtn) {
                var flags = { fr: '🇫🇷 FR', pt: '🇵🇹 PT', en: '🇬🇧 EN' };
                langBtn.textContent = flags[lang] || lang.toUpperCase();
            }
            // Mettre à jour l'item actif dans le dropdown
            var items = document.querySelectorAll('.hs-lang-item');
            for (var l = 0; l < items.length; l++) {
                items[l].classList.toggle('active', items[l].getAttribute('data-lang') === lang);
            }
            // Mettre à jour le meta description
            var metaDesc = document.querySelector('meta[name="description"]');
            if (metaDesc && dict['meta_description']) {
                metaDesc.setAttribute('content', dict['meta_description']);
            }
            // Mettre à jour le title
            if (dict['page_title']) {
                document.title = dict['page_title'];
            }
        });
    }

    /* ── Initialisation ── */
    function init() {
        var locale = getLocale();
        if (locale !== DEFAULT) {
            applyTranslations(locale);
        }
    }

    // Expose globalement
    window.HSi18n = {
        setLocale: setLocale,
        getLocale: getLocale,
        SUPPORTED: SUPPORTED
    };

    // Lancer dès que le DOM est prêt
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }
})();
