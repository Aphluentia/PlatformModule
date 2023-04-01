import React, {useEffect, useState} from 'react';
import QRCode from 'qrcode.react';


const Modules=()=> {
    const [data, setData] = useState(null);
    const [qrCodeUrl, setQrCodeUrl] = useState(null);

    useEffect(() => {
        const posted = async () => {
            await fetch('https://localhost:7176/api/Services', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    webPlatformId: 'string',
                    sessionId: '3fa85f64-5717-4562-b3fc-2c963f66afa6',
                    isValidSession: true,
                }),
            });

            return await fetch('https://localhost:7176/api/Modules?sessionId=3fa85f64-5717-4562-b3fc-2c963f66afa6', );
        };
       
        posted().then((response) => response.json()).then((result) => {
            setData(result.data);
            setQrCodeUrl(result.data.qrCodeData)
        });
        
    }, []);

    
    return (
        <div>
            <h1>Modules Dashboard</h1>
            <br/>
            <div>
                <QRCode value={qrCodeUrl} />
                {data ? 
                    <p>
                        {JSON.stringify(data)}
                    </p> : <p>Loading...</p>}

            </div>
        </div>
    );
}
export default Modules;
