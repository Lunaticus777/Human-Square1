/* Horizonte Atlântico — interactions */
(function () {
  var H = window.HA || {};
  var $ = function (s, r) { return (r || document).querySelector(s); };
  var $$ = function (s, r) { return Array.prototype.slice.call((r || document).querySelectorAll(s)); };

  // Langue mémorisée (évite la redirection automatique ensuite)
  $$('.langs a').forEach(function (a) {
    a.addEventListener('click', function () { try { localStorage.setItem('ha_lang', a.dataset.lang); } catch (e) {} });
  });
  try { if (!localStorage.getItem('ha_lang')) localStorage.setItem('ha_lang', H.t && H.t.lang || 'pt'); } catch (e) {}

  // Nav
  var nav = $('#nav');
  var onScroll = function () { nav.classList.toggle('solid', window.scrollY > 40); };
  onScroll(); window.addEventListener('scroll', onScroll, { passive: true });
  $('#burger').addEventListener('click', function () { nav.classList.toggle('open'); });
  $$('#menu a').forEach(function (a) { a.addEventListener('click', function () { nav.classList.remove('open'); }); });

  // Reveal
  if ('IntersectionObserver' in window) {
    var io = new IntersectionObserver(function (es) {
      es.forEach(function (en) { if (en.isIntersecting) { en.target.classList.add('in'); io.unobserve(en.target); } });
    }, { rootMargin: '0px 0px -8% 0px', threshold: 0.08 });
    $$('.reveal').forEach(function (el, i) { el.style.transitionDelay = (i % 4) * 70 + 'ms'; io.observe(el); });
  } else { $$('.reveal').forEach(function (el) { el.classList.add('in'); }); }

  // Onglets plans
  $$('.tab').forEach(function (b) {
    b.addEventListener('click', function () {
      $$('.tab').forEach(function (x) { x.classList.toggle('on', x === b); });
      $$('.plan').forEach(function (p) { p.hidden = p.dataset.pane !== b.dataset.tab; });
    });
  });

  // Filtres lots
  $$('.flt').forEach(function (b) {
    b.addEventListener('click', function () {
      $$('.flt').forEach(function (x) { x.classList.toggle('on', x === b); });
      var f = b.dataset.f;
      $$('#lots tbody tr').forEach(function (tr) { tr.classList.toggle('hide', f !== 'all' && tr.dataset.state !== f); });
    });
  });

  // Choix d'un lot -> formulaire
  $$('.lot-cta').forEach(function (a) {
    a.addEventListener('click', function () {
      var sel = $('#f-lot'); if (sel) sel.value = ('0' + a.dataset.lot).slice(-2);
    });
  });

  // Lightbox
  var lb = $('#lb'), img = $('#lb-img'), cap = $('#lb-cap'), cur = 0, gal = H.gal || [];
  function show(i) { cur = (i + gal.length) % gal.length; img.src = gal[cur].src; img.alt = gal[cur].alt; cap.textContent = gal[cur].alt + '  ·  ' + (cur + 1) + ' / ' + gal.length; }
  function open(i) { show(i); lb.hidden = false; document.body.style.overflow = 'hidden'; }
  function close() { lb.hidden = true; document.body.style.overflow = ''; }
  $$('[data-i]').forEach(function (el) { el.addEventListener('click', function () { open(+el.dataset.i); }); });
  $$('[data-open]').forEach(function (el) { el.addEventListener('click', function () { open(+el.dataset.open); }); });
  $('.lb-x').addEventListener('click', close);
  $('.lb-p').addEventListener('click', function (e) { e.stopPropagation(); show(cur - 1); });
  $('.lb-n').addEventListener('click', function (e) { e.stopPropagation(); show(cur + 1); });
  lb.addEventListener('click', function (e) { if (e.target === lb) close(); });
  document.addEventListener('keydown', function (e) {
    if (lb.hidden) return;
    if (e.key === 'Escape') close(); if (e.key === 'ArrowRight') show(cur + 1); if (e.key === 'ArrowLeft') show(cur - 1);
  });
  var sx = 0;
  lb.addEventListener('touchstart', function (e) { sx = e.touches[0].clientX; }, { passive: true });
  lb.addEventListener('touchend', function (e) { var d = e.changedTouches[0].clientX - sx; if (Math.abs(d) > 50) show(cur + (d < 0 ? 1 : -1)); });

  // Formulaire -> email pré-rempli (+ WhatsApp)
  var form = $('#form');
  function body() {
    var t = H.t || {};
    var lot = form.lot.value ? t.lot + ' ' + form.lot.value : '';
    return [form.msg.value || form.msg.placeholder, '', t.name + ': ' + form.name.value, t.email + ': ' + form.email.value, t.phone + ': ' + form.phone.value, lot].filter(Boolean).join('\n');
  }
  form.addEventListener('submit', function (e) {
    e.preventDefault();
    var ok = true;
    ['name', 'email'].forEach(function (n) {
      var el = form[n], v = el.value.trim(), bad = !v || (n === 'email' && !/^[^@\s]+@[^@\s]+\.[^@\s]+$/.test(v));
      el.classList.toggle('invalid', bad); if (bad) ok = false;
    });
    form.consent.parentNode.style.color = form.consent.checked ? '' : '#e08a7a';
    if (!form.consent.checked) ok = false;
    if (!ok) return;
    var subj = (H.t.subject || '') + (form.lot.value ? ' — ' + H.t.lot + ' ' + form.lot.value : '');
    location.href = 'mailto:' + H.email + '?subject=' + encodeURIComponent(subj) + '&body=' + encodeURIComponent(body());
    $('#f-ok').hidden = false;
  });
  $('#wa-btn').addEventListener('click', function () {
    this.href = 'https://wa.me/' + H.wa + '?text=' + encodeURIComponent((H.t.subject || '') + '\n' + body());
  });

  // Parallax bandeau
  var band = $('.band'), bimg = $('.band-img');
  if (band && bimg && !window.matchMedia('(prefers-reduced-motion: reduce)').matches) {
    var par = function () {
      var r = band.getBoundingClientRect(), vh = window.innerHeight;
      if (r.bottom < 0 || r.top > vh) return;
      var p = (r.top + r.height / 2 - vh / 2) / vh;
      bimg.style.transform = 'translate3d(0,' + (p * -8) + '%,0)';
    };
    par(); window.addEventListener('scroll', par, { passive: true });
  }

  var y = $('#yr'); if (y) y.textContent = new Date().getFullYear();
})();
