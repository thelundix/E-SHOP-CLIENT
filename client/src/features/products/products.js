import { fetchData, postData } from '../../api/api.js';

export async function renderProducts() {
  const container = document.getElementById('main-content');

  try {
    const products = await fetchData('products');

    if (!products.length) {
      container.innerHTML = `<p>No products found.</p>`;
      return;
    }

    container.innerHTML = `
      <h2>Products</h2>
      <ul>
        ${products.map(p => `
          <li class="product-item">
            <h3>${p.name}</h3>
            <p><strong>Price:</strong> ${p.price} kr</p>
            <p><strong>Weight:</strong> ${p.weight} g</p>
            <p><strong>Package Quantity:</strong> ${p.packageQuantity} st</p>
            <p><strong>Expiry Date:</strong> ${p.expiryDate}</p>
            <p><strong>Manufacture Date:</strong> ${p.manufactureDate}</p>
          </li>
        `).join('')}
      </ul>

      <button id="add-product-btn">Add New Product</button>

      <div id="product-form-container" style="display: none;">
        <form id="add-product-form">
          <p>Name</p><br>
          <input name="name" placeholder="Product Name" required /><br>
          <p>Price</p><br>
          <input name="price" placeholder="Product Price" type="number" required /><br>
          <p>Weight</p><br>
          <input name="weight" placeholder="Product Weight" type="number" required /><br>
          <p>Package Quantity</p><br>
          <input name="packageQuantity" placeholder="Package Quantity" type="number" required /><br>
          <p>Manufacture Date</p><br>
          <input name="manufactureDate" placeholder="Manufacture Date" type="date" required /><br>
          <p>Expiry Date</p><br>
          <input name="expiryDate" placeholder="Expiry Date" type="date" required /><br>
          <button type="submit">Add Product</button>
        </form>
      </div>
    `;

    document.getElementById('add-product-btn').addEventListener('click', () => {
      const formContainer = document.getElementById('product-form-container');
      formContainer.style.display = formContainer.style.display === 'none' ? 'block' : 'none';
    });

    document.getElementById('add-product-form').addEventListener('submit', async (e) => {
      e.preventDefault();
      const form = e.target;
      const newProduct = {
        name: form.name.value,
        price: parseFloat(form.price.value),
        weight: parseFloat(form.weight.value),
        packageQuantity: parseInt(form.packageQuantity.value, 10),
        expiryDate: form.expiryDate.value,
        manufactureDate: form.manufactureDate.value
      };

      await postData('products', newProduct);
      renderProducts(); 
    });

  } catch (err) {
    console.error('Error fetching products:', err);
    container.innerHTML = `<p>Error loading products. Please try again later.</p>`;
  }
}


