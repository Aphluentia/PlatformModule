import './Modules.css'
import {Context} from './../Context';
import {useState, useEffect, useContext} from "react";

export default function Module({ModuleInfo}){
    const [data, setData] = useState(null);

    const [WebPlatformId, setWebPlatformId] = useContext(Context);

    useEffect(() => {
        const interval = setInterval(() => {
            pollMessages();
        }, 10000);

        return () => clearInterval(interval); // clear interval on unmount
    }, []);

    const pollMessages = async () => {
        // make your API call here
        console.log(WebPlatformId);
        fetch(`https://localhost:7048/api/ConnectionManager/Poll?WebPlatformId=${WebPlatformId}&ModuleType=${ModuleInfo.code}`)
            .then(response=>response.json())
            .then(data=> {console.log(data); setData(data);})
    };
    

    return data.messages.length === 0 ? 
            <div className='ModulesCard Disabled'>
                <h2>name: {ModuleInfo.name}</h2>
                <h4>Code: {ModuleInfo.code}</h4>
            </div>
         : 
            <div className='ModulesCard'>
                <h2>name: {ModuleInfo.name}</h2>
                <h4>Code: {ModuleInfo.code}</h4>
                
            </div>
};