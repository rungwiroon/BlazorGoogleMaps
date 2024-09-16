export function BlazorGoogleMaps_LoadScript(path) {
    return new Promise((resolve, reject) => {
        const script = document.createElement('script');
        script.src = path;
        script.type = 'text/javascript';
        script.async = true;

        script.onload = () => {
            resolve();
        };

        script.onerror = (error) => {
            console.error(`Error loading script: ${path}`, error);
            reject(error);
        };

        document.head.appendChild(script);
    });
}