import React, {useEffect, useState} from 'react';
import QRCode from 'qrcode.react';
import Module from './Module.js'


const Modules=(sessionData)=> {
    const [pageData, setPageData] = useState(null);
    useEffect(() => {

        const fetchPageData = async () => {
            var response = await fetch(`https://localhost:7176/api/Dashboard/FetchPageData?sessionId=${sessionData.sessionData.sessionId}`,);
            var jsonified = await response.json();
            setPageData(jsonified.data);
        };
       
        fetchPageData();
        
    }, [sessionData]);

    return (
        <div>
            <h1>Modules Dashboard</h1>
            <br/>
            <div>
                {pageData ? 
                    <p>
                        <QRCode value={pageData.qrCodeData}/>
                    </p> : <p>Loading...</p>}
                {pageData ? 
                    <p>
                        {JSON.stringify(pageData)}
                    </p> : <p>Loading...</p>}
                    
                { pageData && pageData.server ? (
                    <div>
                        {pageData.server.map(function (value, index) {
                            return <Module sessionData={sessionData} webPlatformId={sessionData.webPlatformId} module= {value} key={index} />
                        })}
                        
                    </div>
                ) : (
                    <div>Not Connected</div>
                )}

            </div>
        </div>
    );
}
export default Modules;

