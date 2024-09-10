import { useEffect, useState } from 'react';
import Card from './components/Card'
import Success from './components/SuccessBanner'
import './App.css';

function App() {
    return (
        <div className="App">
            <h1>Payment Integration with Square</h1>
            <Card/>
        </div>
    );
}

export default App;