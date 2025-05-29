import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom'; // Th�m d�ng n�y
import './index.css';
import App from './App.tsx';

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <BrowserRouter> {/* Th�m th? n�y */}
            <App />
        </BrowserRouter>
    </StrictMode>
);
