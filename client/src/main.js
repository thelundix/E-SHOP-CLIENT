// src/main.js
import { renderCustomers } from './features/customers/customers.js';
import { renderProducts } from './features/products/products.js';
import { renderSuppliers } from './features/suppliers/suppliers.js';




document.getElementById('show-products').addEventListener('click', renderProducts);
document.getElementById('show-suppliers').addEventListener('click', renderSuppliers);
document.getElementById('show-customers').addEventListener('click', renderCustomers);


// Initial load
renderProducts();
