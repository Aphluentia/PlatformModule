
import "./Modules.css";
import {useState, useEffect} from "react";
import QRCode from 'qrcode.react';
import Module from './Module';

export default function Modules({sessionData}) {

    const [pageInfo, setPageInfo] = useState(null);
 
    useEffect(() => {
        if (sessionData){
            fetch(`https://localhost:7176/api/Modules/Setup?SessionId=${sessionData.data.sessionId}`)
                .then(response => response.json())
                .then(data => {console.log(data.data.modules);setPageInfo(data.data)})
        }
    }, [sessionData]);

    return (
        <>
            {pageInfo===null? <p>Generating QR Code</p>: <p><QRCode value={pageInfo.qrCodeData}/></p>}
            <hr/>
            { pageInfo === null ? <p>Loading Modules</p>: 
                    <div className='InlineModulesDiv'>
                        { pageInfo.modules.map(function (value, index) {
                            return <Module ModuleInfo={value} key={index}/>
                            })
                        }
                        
                    </div>
                }
            
            
        </>
        )
   
}