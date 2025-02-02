# Project Title
## Vì sao cần DTO
vì dto sẽ tiết kiệm dữ liệu khi nó là class chỉ lấy ra những thuộc tính cần thiết
với API thì ta có Product class thì ta có ProductDTO, ProductCreateDTO, ProductUpdateDTO phục vụ cho mỗi mục đích khác nhau
## DAO là gì ?
DAO sẽ làm việc CRUD trực tiếp với context database
## Repository liên quan gì tới DAO ?
Repo sẽ gọi DAO thực hiện CRUD và có Interface quản lý bản thiết kế các hàm trong repo đó
"Nhờ ai đó làm việc gì" là việc của repo
## 3 layer architecture là gì ?
### Gồm 3 layer
**BusinessObject** chứa models và DBContext
**DataAccess** chứa DAO và Repository (ProductRepository và IProductRepository).
Khi **đi thi** muốn nhanh thì **bỏ qua DAO** và **dùng Repository generic** thay vì tạo từng repo cho từng class
### Set up Auto Mapper

B1: install **automapper.extensions.microsoftdepencyinjection**

B2: go to program and add code
```
builder.Services.AddAutoMapper(typeof(MappingConfig))
```
B3: create class **MappingConfig : Profile**
Nên dùng **ReverseMap()** để code ngắn và nhanh !

Thay vì như thế này
```
CreateMap<ProductDTO, ProductCreateDTO>()
CreateMap<ProductCreateDTO, ProductDTO>
```
thì sẽ như thế này
```
CreateMap<ProductDTO, ProductCreateDTO>().ReverseMap();
```
# API Project
## Standard API response
response API thường theo standard API response, không như bình thường chúng ta trả về một kiểu chay như trả về một đối tượng hay list

Tạo class **APIResponse**
```
public class APIResponse 
{ 
public HttpStatusCode StatusCode { get; set; } 
public bool IsSuccess { get; set; } 
public List<string> ErrorMessages { get; set; } 
public object Result { get; set; }
}
```
reponseAPI không phải service nên không được addscoped vào program. ĐIều đó dẫn đến việc khi khởi tạo không được cho vào tham số của contructor của class mà phải khởi tạo class
```
  protected APIResponse _response;
```
RỒi trong contructor khởi tạo lệnh `new()`
```
 _response = new();
```
## Addscoped và dbcontext khi làm với API vào program
1. Add DBContext, nếu không sẽ báo lỗi không có dbcontext

    1.1 Phải có appsettings với `copy always` properties và có ConnectionStrings
    ```
       "ConnectionStrings": {
   "MyStoreDB": "Data Source=(local);Database=Ass01_API;User Id=***;Password=***;TrustServerCertificate=true;Trusted_Connection=SSPI;Encrypt=false;"
 }
    ```
    1.2 Thêm DBcontext vào program :
    ```
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("MyStoreDB"));
    });
    ```

2. AddScoped cho Repository và cả DAO nếu không sẽ lỗi lifetime
3. Có thể debug lỗi bằng cách thêm code trước `builder.build()`:
```
using (var scope = serviceProvider.CreateScope())
{
    var test = scope.ServiceProvider.GetService<IProductRepository>();
    if (test == null)
        Console.WriteLine("IProductRepository chưa được đăng ký đúng!");
}
```
