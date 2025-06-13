// MSAL Cache Management Helper
window.authHelper = {
    clearMsalCache: function() {
        try {
            // MSAL-spezifische Cache-Keys löschen
            const keys = Object.keys(localStorage);
            keys.forEach(key => {
                if (key.startsWith('msal.') || 
                    key.startsWith('msal-') || 
                    key.includes('msal') ||
                    key.includes('azure') ||
                    key.includes('login.microsoftonline.com')) {
                    localStorage.removeItem(key);
                }
            });
            
            // Session Storage auch leeren
            const sessionKeys = Object.keys(sessionStorage);
            sessionKeys.forEach(key => {
                if (key.startsWith('msal.') || 
                    key.startsWith('msal-') || 
                    key.includes('msal') ||
                    key.includes('azure') ||
                    key.includes('login.microsoftonline.com')) {
                    sessionStorage.removeItem(key);
                }
            });
            
            console.log('MSAL Cache erfolgreich geleert');
            return true;
        } catch (error) {
            console.error('Fehler beim Leeren des MSAL Cache:', error);
            return false;
        }
    },
    
    isInPopup: function() {
        try {
            // Explizit Boolean zurückgeben
            return Boolean(window.opener && window.opener !== window);
        } catch (error) {
            console.error('Fehler bei Popup-Erkennung:', error);
            return false;
        }
    },
    
    isInIframe: function() {
        try {
            // Explizit Boolean zurückgeben
            return Boolean(window.self !== window.top);
        } catch (error) {
            console.error('Fehler bei Iframe-Erkennung:', error);
            return true; // Assume iframe if we can't check
        }
    },
    
    forceReload: function() {
        try {
            if (this.isInPopup()) {
                // Wenn wir in einem Popup sind, schließe es und lade die Hauptseite neu
                window.close();
                if (window.opener) {
                    window.opener.location.reload();
                }
            } else {
                window.location.reload(true);
            }
            return true;
        } catch (error) {
            console.error('Fehler beim Neuladen:', error);
            return false;
        }
    },
    
    redirectToLogin: function() {
        try {
            if (this.isInPopup()) {
                // Popup schließen und Hauptfenster zur Login-Seite weiterleiten
                if (window.opener) {
                    window.opener.location.href = '/authentication/login';
                    window.close();
                }
            } else {
                window.location.href = '/authentication/login';
            }
            return true;
        } catch (error) {
            console.error('Fehler beim Weiterleiten zum Login:', error);
            return false;
        }
    },
    
    handleAuthError: function() {
        try {
            console.log('Auth-Fehler erkannt, bereinige Cache und leite weiter...');
            this.clearMsalCache();
            
            if (this.isInPopup()) {
                // Popup schließen und Hauptfenster neu laden
                if (window.opener) {
                    window.opener.location.reload();
                    window.close();
                }
            } else {
                // Hauptfenster zur Startseite weiterleiten
                window.location.href = '/';
            }
            return true;
        } catch (error) {
            console.error('Fehler beim Behandeln des Auth-Fehlers:', error);
            return false;
        }
    }
}; 