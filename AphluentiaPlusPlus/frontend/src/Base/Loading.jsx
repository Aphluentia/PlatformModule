import './Loading.css';
import { InfinitySpin } from  'react-loader-spinner'
const Loading = () => {
    return (
        <div className='loading-div'>
            <InfinitySpin 
                className = "loading-spinner"
            /> 
        </div>
    );
}

export default Loading;
